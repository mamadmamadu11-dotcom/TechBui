using Microsoft.Extensions.Logging;
using TechBui.Helpers;
using TechBui.ViewModels;
using TechBui.Services;

namespace TechBui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ثبت Converter به عنوان Resource
        builder.Services.AddSingleton<InvertBoolConverter>();

        // ثبت ViewModel و Service
        builder.Services.AddSingleton<ChatViewModel>();
        builder.Services.AddSingleton<ChatService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}