<div align="center">

<img src="Resources/Images/logo.png" alt="TechBui Logo" width="120" />

# 🤖 TechBui - AI Chat Assistant

**A modern, cross-platform AI chatbot powered by Claude API & .NET MAUI**

[![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-6C5CE7?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/en-us/apps/maui)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Claude](https://img.shields.io/badge/Claude%20API-36E4F4?style=for-the-badge&logo=anthropic&logoColor=black)](https://freemodel.dev)
[![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Android%20%7C%20iOS-blue?style=for-the-badge)]()

</div>

---

## 📸 Screenshots

<div align="center">

| Dark Mode | Chat Interface |
|:---------:|:--------------:|
| ![Dark](screenshots/dark.png) | ![Chat](screenshots/chat.png) |

</div>

---

## ✨ Features

- 🎨 **Premium Dark UI** with Glassmorphism & Neon Accents
- 🤖 **AI-Powered** using Claude Sonnet, Opus & Haiku models
- 🌍 **Bilingual Support** - Persian (Farsi) & English
- 📱 **Cross-Platform** - Windows, Android, iOS
- 💬 **Smart Fallback System** - Auto-switches between 5 AI models & 2 API formats
- 🔑 **Secure API Key Storage** using device Preferences
- 🎯 **Right-to-Left (RTL)** support for Persian
- ⚡ **Auto Fallback** - If one model fails, tries 9 other combinations
- 📝 **Conversation History** with smooth animations
- 🗑️ **Clear Chat** functionality
- 🔄 **Retry Mechanism** for failed requests
- 🎨 **Modern UI** with gradients, shadows & custom icons

---

## 🛠️ Tech Stack
.NET 9 MAUI • C# 12 • XAML • MVVM Architecture
Anthropic Claude API • OpenAI Compatible API
HttpClient • JSON • REST APIs
Visual Studio 2022 • Git

text

---

## 📦 Supported AI Models

| Model | Speed | Quality | Status |
|-------|-------|---------|--------|
| `claude-sonnet-5` | 🚀 Fast | ⭐ Excellent | Active |
| `claude-sonnet-4-6` | 🚀 Fast | ⭐ Very Good | Fallback |
| `claude-opus-4-7` | 🐢 Slow | 🧠 Best | Fallback |
| `claude-haiku-4-5` | ⚡ Fastest | 👌 Good | Fallback |
| `auto` | 🔄 Auto | 🎲 Dynamic | Last Resort |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (with .NET MAUI workload)
- [FreeModel.dev API Key](https://freemodel.dev) (Free Tier available)

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/YOUR_USERNAME/TechBui.git
cd TechBui
Open in Visual Studio 2022

text
Open TechBui.sln
Build & Run

text
Build > Rebuild Solution
Press F5 to run
Get API Key

Visit freemodel.dev

Sign up and create an API key (starts with fe_oa_)

Enter the key in the app

🏗️ Project Structure
text
TechBui/
├── Models/
│   └── ChatMessage.cs          # Data models for chat
├── Services/
│   └── ChatService.cs          # AI API integration with fallback
├── ViewModels/
│   └── ChatViewModel.cs        # MVVM ViewModel
├── Helpers/
│   └── InvertBoolConverter.cs  # XAML value converter
├── Resources/
│   ├── AppIcon/                # App icons
│   ├── Splash/                 # Splash screen
│   ├── Images/                 # Logo & SVG icons
│   └── Fonts/                  # Custom fonts
├── Platforms/
│   ├── Android/
│   └── iOS/
├── MainPage.xaml               # Main chat UI
├── MainPage.xaml.cs            # Code-behind
├── App.xaml                    # Application resources
├── AppShell.xaml               # Shell navigation
├── MauiProgram.cs              # Dependency injection
└── TechBui.csproj              # Project configuration
🎨 Customization
Change Color Theme
Edit color values in MainPage.xaml:

xml
<GradientStop Color="#36E4F4" Offset="0.0"/>  <!-- Cyan -->
<GradientStop Color="#6C5CE7" Offset="1.0"/>  <!-- Purple -->
Change AI Model Priority
Edit the fallback list in Services/ChatService.cs:

csharp
private readonly string[] _modelFallbackList = new string[]
{
    "claude-sonnet-5",           // Primary model
    "claude-sonnet-4-6",         // First fallback
    // Add your preferred models here
};
Add More Languages
Modify the system prompt in ChatService.cs:

csharp
system = "You are a helpful assistant. Reply in the user's language."
🤝 Contributing
Contributions are welcome! Feel free to:

🐛 Report bugs via Issues

💡 Suggest features via Discussions

🔧 Submit pull requests

📝 License
This project is licensed under the MIT License - see the LICENSE file for details.

👨‍💻 Author
MohammadReza Ghanbari

https://img.shields.io/badge/Instagram-@techbui-E4405F?style=flat&logo=instagram&logoColor=white
https://img.shields.io/badge/Email-mamadmamadu11@gmail.com-D14836?style=flat&logo=gmail&logoColor=white
https://img.shields.io/badge/Phone-09930533371-25D366?style=flat&logo=whatsapp&logoColor=white

⭐ Support
If you find this project useful, please give it a star ⭐ and share it with others!

<div align="center">
Built with ❤️ for the developer community

</div>
