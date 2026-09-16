using AI.CustomerSupport.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AI.CustomerSupport.Services
{
    public class ChatService
    {
        private readonly OpenAIService _openAIService;
        private readonly SearchService _searchService;
        private readonly TicketService _ticketService;

        public ChatService(
            OpenAIService openAIService,
            SearchService searchService,
            TicketService ticketService)
        {
            _openAIService = openAIService;
            _searchService = searchService;
            _ticketService = ticketService;
        }

        public async Task<ChatResponse> GetChatResponseAsync(ChatRequest request)
        {
            // ==========================================
            // 1. Check whether the question contains
            //    a ticket number
            // ==========================================

            var ticketMatch = Regex.Match(
                request.Message,
                @"\b(?:ticket|case)\s*#?\s*(\d+)\b",
                RegexOptions.IgnoreCase);

            if (ticketMatch.Success)
            {
                int ticketId = int.Parse(ticketMatch.Groups[1].Value);

                // Get ticket from SQL Server
                var ticket =
                    await _ticketService.GetTicketByIdAsync(ticketId);

                if (ticket == null)
                {
                    return new ChatResponse
                    {
                        Answer = $"Ticket {ticketId} was not found."
                    };
                }

                // Convert ticket information to JSON
                var ticketContext =
                    JsonSerializer.Serialize(ticket);

                // Ask Llama to answer using ONLY ticket information
                var ticketPrompt = $"""
                    You are a customer support assistant.

                    Answer the customer's question using ONLY the
                    ticket information provided below.

                    Do not invent or assume any information.

                    <ticket_information>
                    {ticketContext}
                    </ticket_information>

                    Customer question:
                    {request.Message}

                    Answer:
                    """;

                var ticketAnswer =
                    await _openAIService.GetResponseAsync(ticketPrompt);

                return new ChatResponse
                {
                    Answer = ticketAnswer
                };
            }

            // ==========================================
            // 2. No ticket number found
            //    Use the RAG knowledge base
            // ==========================================

            var knowledge =
                await _searchService.SearchAsync(request.Message);

            if (knowledge.Count == 0)
            {
                return new ChatResponse
                {
                    Answer =
                        "I don't have that information in our company knowledge base."
                };
            }

            // Convert retrieved chunks into text
            var context = string.Join(
                "\n\n",
                knowledge.Select(chunk => chunk.Text));

            // TEMPORARY DEBUG
            Console.WriteLine("====================================");
            Console.WriteLine("QUESTION:");
            Console.WriteLine(request.Message);
            Console.WriteLine("====================================");
            Console.WriteLine("RETRIEVED CONTEXT:");
            Console.WriteLine(context);
            Console.WriteLine("====================================");

            // Create RAG prompt
            var prompt = $"""
                You are a customer support assistant.

                Answer the customer question using ONLY the information
                between the <company_information> tags.

                <company_information>
                {context}
                </company_information>

                Customer question:
                {request.Message}

                If the company information does not contain the answer,
                respond exactly:
                I don't have that information in our company knowledge base.

                Answer:
                """;

            // Send prompt to Llama
            var answer =
                await _openAIService.GetResponseAsync(prompt);

            return new ChatResponse
            {
                Answer = answer
            };
        }
    }
}