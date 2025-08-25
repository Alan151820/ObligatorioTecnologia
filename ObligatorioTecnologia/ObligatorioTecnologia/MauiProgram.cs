using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ObligatorioTecnologia.Data;
using ObligatorioTecnologia.Modelo;
using ObligatorioTecnologia.Services;
using Microsoft.Maui.Controls.Maps;



namespace ObligatorioTecnologia
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
            const string currencyLayerApiKey = "126fd50e823ed9d2ef76d90b0a3fe9ab";
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "sponsors.db3");
            builder.Services.AddSingleton(new SponsorDb(dbPath));
            builder.Services.AddTransient<SponsorsPage>();
            builder.Services.AddTransient<SponsorFormPage>();

            builder.Services.AddSingleton(new HttpClient());
            builder.Services.AddSingleton(sp =>
                new CurrencyLayerClient(sp.GetRequiredService<HttpClient>(), currencyLayerApiKey));

            builder.Services.AddTransient<CotizacionViewModel>();
            builder.Services.AddTransient<CotizacionesPage>();
            builder.Logging.AddDebug();
            builder.Services.AddSingleton<SponsorsPage>();
            
#endif


            return builder.Build();
        }
    }
}
