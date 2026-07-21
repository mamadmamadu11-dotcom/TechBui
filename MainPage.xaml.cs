using TechBui.ViewModels;

namespace TechBui;

public partial class MainPage : ContentPage
{
    private readonly ChatViewModel _viewModel;

    public MainPage()
    {
        InitializeComponent();
        _viewModel = new ChatViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // تنظیم SoftInputMode برای ثابت نگه داشتن هدر
#if ANDROID
        if (Window?.Handler?.PlatformView is Android.App.Activity activity)
        {
            activity.Window?.SetSoftInputMode(
                Android.Views.SoftInput.AdjustResize | 
                Android.Views.SoftInput.StateHidden
            );
        }
#elif IOS
        // برای iOS از KeyboardAutoManagerScroll استفاده می‌کنیم
#endif
    }
}