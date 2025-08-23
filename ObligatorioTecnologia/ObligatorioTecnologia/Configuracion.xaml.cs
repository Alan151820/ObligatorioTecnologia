namespace ObligatorioTecnologia;

public partial class Configuracion : ContentPage
{
    public Configuracion()
    {
        InitializeComponent();

        LoadPreferences();
    }

    private void LoadPreferences()
    {
        // Cargar toggles desde Preferences
        SwitchClima.IsToggled = Preferences.Get("MenuClimaVisible", true);
        SwitchCotizaciones.IsToggled = Preferences.Get("MenuCotizacionesVisible", true);
        SwitchCine.IsToggled = Preferences.Get("MenuCineVisible", true);

        // Actualizar colores de las tarjetas
        UpdateCardColors();

        // Mantener siempre visible el frame en el layout
        MenuClimaCard.IsVisible = true;
        MenuCotizacionesCard.IsVisible = true;
        MenuCineCard.IsVisible = true;

    }

    private void UpdateCardColors()
    {
        // Cambia el color de fondo de cada tarjeta según esté activa
        MenuClimaCard.BackgroundColor = SwitchClima.IsToggled ? Colors.LightGreen : Colors.White;
        MenuCotizacionesCard.BackgroundColor = SwitchCotizaciones.IsToggled ? Colors.LightGreen : Colors.White;
        MenuCineCard.BackgroundColor = SwitchCine.IsToggled ? Colors.LightGreen : Colors.White;
    }

    private void TapClima_Tapped(object sender, EventArgs e)
    {
        SwitchClima.IsToggled = !SwitchClima.IsToggled;
        Preferences.Set("MenuClimaVisible", SwitchClima.IsToggled);
        UpdateCardColors();
        (Shell.Current as AppShell)?.ConfigurarMenu();
    }

    private void TapCotizaciones_Tapped(object sender, EventArgs e)
    {
        SwitchCotizaciones.IsToggled = !SwitchCotizaciones.IsToggled;
        Preferences.Set("MenuCotizacionesVisible", SwitchCotizaciones.IsToggled);
        UpdateCardColors();
        (Shell.Current as AppShell)?.ConfigurarMenu();
    }

    private void TapCine_Tapped(object sender, EventArgs e)
    {
        SwitchCine.IsToggled = !SwitchCine.IsToggled;
        Preferences.Set("MenuCineVisible", SwitchCine.IsToggled);
        UpdateCardColors();
        (Shell.Current as AppShell)?.ConfigurarMenu();
    }
}

