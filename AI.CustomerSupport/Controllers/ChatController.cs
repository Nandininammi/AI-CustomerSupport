using AI.CustomerSupport.Models;
using AI.CustomerSupport.Services;
using Microsoft.AspNetCore.Mvc;

namespace AI.CustomerSupport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController:ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        // [HttpPost]
        //public async Task<ActionResult<ChatResponse>> Chat(ChatRequest request)
        //{
        //    var response = await _chatService.GetChatResponseAsync(request);

        //    return Ok(response);
        //}


        [HttpPost]
        public async Task<ActionResult<ChatResponse>> Chat([FromBody] ChatRequest request)
        {
            Console.WriteLine("========== CHAT CONTROLLER START ==========");
            Console.WriteLine($"Message: {request.Message}");

            var response = await _chatService.GetChatResponseAsync(request);

            Console.WriteLine("========== CHAT CONTROLLER RESPONSE ==========");
            Console.WriteLine($"Answer: {response.Answer}");

            return Ok(response);
        }


    }
    
    
}
