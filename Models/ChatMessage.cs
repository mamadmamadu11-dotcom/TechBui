using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechBui.Models
{
    public class ChatMessage
    {
        public string Role { get; set; } = string.Empty;  // "user", "assistant", "system"
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool IsUser => Role == "user";
    }

    public class ChatRequest
    {
        public string Model { get; set; } = "claude-sonnet-5";
        public List<ChatMessage> Messages { get; set; } = new();
        public int MaxTokens { get; set; } = 2000;
        public double Temperature { get; set; } = 0.7;
    }

    public class ChatResponse
    {
        public List<ChatChoice> Choices { get; set; } = new();
    }

    public class ChatChoice
    {
        public ChatMessage Message { get; set; } = new();
    }

    public class ApiSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://cc.freemodel.dev/v1/chat/completions";
        public string Model { get; set; } = "claude-sonnet-5";
    }
}