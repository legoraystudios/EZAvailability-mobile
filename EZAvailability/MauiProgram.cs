using Microsoft.Extensions.Logging;

using CommunityToolkit.Maui;
using ZXing.Net.Maui.Controls;
using Plugin.Maui.Audio;
using EZAvailability.Views;

namespace EZAvailability
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-brands-400.otf", "FABrands");
                    fonts.AddFont("bootstrap-icons.ttf", "FABootstrap");
                    fonts.AddFont("fa-regular-400.otf", "FARegular");
                    fonts.AddFont("fa-solid-900.otf", "FASolid");
                });

            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddTransient<ScanProductView>();
            builder.Services.AddTransient<Views.ProductView, ViewModel.ProductViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
