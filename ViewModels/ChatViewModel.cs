using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using TechBui.Models;
using TechBui.Services;

namespace TechBui.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        private readonly ChatService _chatService;
        private string _userMessage = string.Empty;
        private string _apiKey = string.Empty;
        private bool _isBusy;
        private bool _isApiKeyValid;
        private string _statusText = "آماده";
        private string _errorMessage = string.Empty;
        private bool _hasError;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ChatViewModel()
        {
            _chatService = new ChatService();
            Messages = new ObservableCollection<ChatMessage>();

            // لود کلید API ذخیره شده
            ApiKey = _chatService.GetApiKey();
            IsApiKeyValid = !string.IsNullOrEmpty(ApiKey);

            // تعریف کامندها
            SendMessageCommand = new Command(async () => await SendMessageAsync(), () => !IsBusy);
            ClearChatCommand = new Command(ClearChat);
            SaveApiKeyCommand = new Command(SaveApiKey);
            RetryCommand = new Command(async () => await SendMessageAsync());
        }

        // ============ Properties ============
        public ObservableCollection<ChatMessage> Messages { get; set; }

        public string UserMessage
        {
            get => _userMessage;
            set
            {
                _userMessage = value;
                OnPropertyChanged();
            }
        }

        public string ApiKey
        {
            get => _apiKey;
            set
            {
                _apiKey = value;
                OnPropertyChanged();
                IsApiKeyValid = !string.IsNullOrEmpty(value);
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                ((Command)SendMessageCommand).ChangeCanExecute();
            }
        }

        public bool IsApiKeyValid
        {
            get => _isApiKeyValid;
            set
            {
                _isApiKeyValid = value;
                OnPropertyChanged();
            }
        }

        public string StatusText
        {
            get => _statusText;
            set
            {
                _statusText = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                HasError = !string.IsNullOrEmpty(value);
            }
        }

        public bool HasError
        {
            get => _hasError;
            set
            {
                _hasError = value;
                OnPropertyChanged();
            }
        }

        // ============ Commands ============
        public ICommand SendMessageCommand { get; }
        public ICommand ClearChatCommand { get; }
        public ICommand SaveApiKeyCommand { get; }
        public ICommand RetryCommand { get; }

        // ============ Methods ============
        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(UserMessage)) return;
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                ErrorMessage = "لطفاً ابتدا کلید API را وارد کنید";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;
            StatusText = "در حال ارسال...";

            var messageText = UserMessage;
            UserMessage = string.Empty;

            // اضافه کردن پیام کاربر به UI
            var userChatMessage = new ChatMessage
            {
                Role = "user",
                Content = messageText,
                Timestamp = DateTime.Now
            };
            Messages.Add(userChatMessage);

            try
            {
                StatusText = "در انتظار پاسخ...";

                // ارسال به سرویس
                var response = await _chatService.SendMessageAsync(messageText);

                // اضافه کردن پاسخ AI به UI
                var aiChatMessage = new ChatMessage
                {
                    Role = "assistant",
                    Content = response,
                    Timestamp = DateTime.Now
                };
                Messages.Add(aiChatMessage);

                StatusText = "آماده";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                StatusText = "خطا در ارتباط";

                // اضافه کردن پیام خطا
                Messages.Add(new ChatMessage
                {
                    Role = "assistant",
                    Content = $"❌ {ex.Message}",
                    Timestamp = DateTime.Now
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ClearChat()
        {
            Messages.Clear();
            _chatService.ClearHistory();
            ErrorMessage = string.Empty;
            StatusText = "آماده";
        }

        private void SaveApiKey()
        {
            if (!string.IsNullOrWhiteSpace(ApiKey))
            {
                _chatService.SetApiKey(ApiKey);
                StatusText = "✅ کلید API ذخیره شد";
                ErrorMessage = string.Empty;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}