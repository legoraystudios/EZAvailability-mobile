using Microsoft.Extensions.Logging;

using CommunityToolkit.Maui;

namespace EZAvailability
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
