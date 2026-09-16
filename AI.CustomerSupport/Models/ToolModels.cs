namespace AI.CustomerSupport.Models
{
    public class ToolCall
    {
        public string Name { get; set; } = string.Empty;

        public Dictionary<string, object> Arguments { get; set; }
            = new();
    }
}