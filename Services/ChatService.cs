using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TechBui.Models;

namespace TechBui.Services
{
    public class ChatService
    {
        private readonly HttpClient _httpClient;
        private string _apiKey;
        private List<ChatMessage> _conversationHistory;

        // لیست مدل‌ها به ترتیب اولویت (از بهترین به سریع‌ترین)
        private readonly string[] _modelFallbackList = new string[]
        {
            "claude-sonnet-5",              // مدل اصلی
            "claude-sonnet-4-6",            // مدل قبلی
            "claude-opus-4-7",              // مدل قدرتمند
            "claude-haiku-4-5-20251001",   // مدل سریع و پایدار
            "auto"                          // انتخاب خودکار توسط سرور
        };

        // لیست endpointها با فرمت‌های مختلف
        private readonly (string url, string format)[] _endpointFallbackList = new (string, string)[]
        {
            ("https://cc.freemodel.dev/v1/messages", "anthropic"),           // Anthropic Native
            ("https://api.freemodel.dev/v1/chat/completions", "openai"),     // OpenAI Compatible
        };

        public ChatService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(2);
            _apiKey = string.Empty;
            _conversationHistory = new List<ChatMessage>();
        }

        public void SetApiKey(string apiKey)
        {
            _apiKey = apiKey;
            Preferences.Set("techbui_api_key", apiKey);
        }

        public string GetApiKey()
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                _apiKey = Preferences.Get("techbui_api_key", "");
            }
            return _apiKey;
        }

        public void ClearHistory()
        {
            _conversationHistory.Clear();
        }

        public List<ChatMessage> GetHistory()
        {
            return _conversationHistory;
        }

        public async Task<string> SendMessageAsync(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new Exception("کلید API تنظیم نشده است");
            }

            // اضافه کردن پیام کاربر به تاریخچه
            _conversationHistory.Add(new ChatMessage
            {
                Role = "user",
                Content = userMessage,
                Timestamp = DateTime.Now
            });

            string lastError = "";

            // ===== تلاش با مدل‌های مختلف =====
            foreach (var model in _modelFallbackList)
            {
                // ===== تلاش با endpointهای مختلف =====
                foreach (var (url, format) in _endpointFallbackList)
                {
                    try
                    {
                        Console.WriteLine($"🔄 تلاش با مدل: {model} | فرمت: {format}");

                        string response;

                        if (format == "anthropic")
                        {
                            response = await TryAnthropicFormat(url, model);
                        }
                        else // openai
                        {
                            response = await TryOpenAIFormat(url, model);
                        }

                        if (!string.IsNullOrEmpty(response))
                        {
                            // موفقیت! اضافه کردن پاسخ به تاریخچه
                            _conversationHistory.Add(new ChatMessage
                            {
                                Role = "assistant",
                                Content = response,
                                Timestamp = DateTime.Now
                            });

                            Console.WriteLine($"✅ موفقیت با مدل: {model} | فرمت: {format}");
                            return response;
                        }
                    }
                    catch (Exception ex)
                    {
                        lastError = ex.Message;
                        Console.WriteLine($"❌ شکست با مدل: {model} | فرمت: {format} | خطا: {lastError}");
                        // ادامه به مدل/endpoint بعدی
                        continue;
                    }
                }
            }

            // اگه هیچکدوم جواب ندادن
            throw new Exception($"همه مدل‌ها و endpointها ناموفق بودند.\n\nآخرین خطا: {lastError}\n\nلطفاً اتصال اینترنت را بررسی کنید یا چند دقیقه بعد تلاش کنید.");
        }

        // ===== Anthropic Format =====
        private async Task<string> TryAnthropicFormat(string url, string model)
        {
            var anthropicMessages = new List<object>();
            foreach (var msg in _conversationHistory)
            {
                anthropicMessages.Add(new
                {
                    role = msg.Role,
                    content = msg.Content
                });
            }

            var requestBody = new
            {
                model = model,
                max_tokens = 2000,
                system = "You are a smart, helpful AI assistant. Reply in the same language the user uses. If Persian, reply in natural fluent Persian. Be concise but complete.",
                messages = anthropicMessages.ToArray()
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var jsonContent = JsonSerializer.Serialize(requestBody, jsonOptions);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            request.Headers.Add("anthropic-version", "2023-06-01");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"{response.StatusCode}: {errorBody}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();

            // Parse پاسخ Anthropic
            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            string aiResponse = "";
            if (root.TryGetProperty("content", out var contentArray))
            {
                foreach (var block in contentArray.EnumerateArray())
                {
                    if (block.TryGetProperty("type", out var type) && type.GetString() == "text")
                    {
                        aiResponse += block.GetProperty("text").GetString();
                    }
                }
            }

            if (string.IsNullOrEmpty(aiResponse))
            {
                throw new Exception("پاسخ خالی از سرور (Anthropic format)");
            }

            return aiResponse;
        }

        // ===== OpenAI Format =====
        private async Task<string> TryOpenAIFormat(string url, string model)
        {
            var messages = new List<object>
            {
                new
                {
                    role = "system",
                    content = "You are a smart, helpful AI assistant. Reply in the same language the user uses. If Persian, reply in natural fluent Persian. Be concise but complete."
                }
            };

            foreach (var msg in _conversationHistory)
            {
                messages.Add(new
                {
                    role = msg.Role,
                    content = msg.Content
                });
            }

            var requestBody = new
            {
                model = model,
                messages = messages.ToArray(),
                max_tokens = 2000,
                temperature = 0.7
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var jsonContent = JsonSerializer.Serialize(requestBody, jsonOptions);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"{response.StatusCode}: {errorBody}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();

            // Parse پاسخ OpenAI
            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            string aiResponse = "";
            if (root.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
            {
                var choice = choices[0];
                if (choice.TryGetProperty("message", out var message))
                {
                    aiResponse = message.GetProperty("content").GetString() ?? "";
                }
                else if (choice.TryGetProperty("text", out var text))
                {
                    aiResponse = text.GetString() ?? "";
                }
            }

            if (string.IsNullOrEmpty(aiResponse))
            {
                throw new Exception("پاسخ خالی از سرور (OpenAI format)");
            }

            return aiResponse;
        }
    }
}