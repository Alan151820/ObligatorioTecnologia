using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ObligatorioTecnologia.Data;
using ObligatorioTecnologia.Modelo;
using ObligatorioTecnologia.Services;



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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
            const string currencyLayerApiKey = "126fd50e823ed9d2ef76d90b0a3fe9ab";

            builder.Services.AddSingleton(new HttpClient());
            builder.Services.AddSingleton(sp =>
                new CurrencyLayerClient(sp.GetRequiredService<HttpClient>(), currencyLayerApiKey));

            builder.Services.AddTransient<CotizacionViewModel>();
            builder.Services.AddTransient<CotizacionesPage>();
            builder.Logging.AddDebug();
            builder.Services.AddSingleton<AppDatabase>();
            builder.Services.AddSingleton<SponsorsPage>();
            builder.Services.AddSingleton<SponsorRepository>();
            builder.Services.AddSingleton<PlanRepository>();
            builder.Services.AddSingleton<PostRepository>();
#endif


            return builder.Build();
        }
    }
}
