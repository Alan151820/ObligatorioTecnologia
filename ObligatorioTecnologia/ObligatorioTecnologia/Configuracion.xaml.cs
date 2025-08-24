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
        SwitchClima.IsToggled = Preferences.Get("MenuClimaVisible", true);
        SwitchCotizaciones.IsToggled = Preferences.Get("MenuCotizacionesVisible", true);
        SwitchCine.IsToggled = Preferences.Get("MenuCineVisible", true);
        SwitchPatrocinadores.IsToggled = Preferences.Get("MenuPatrocinadoresVisible", true);

        UpdateCardColors();

        MenuClimaCard.IsVisible = true;
        MenuCotizacionesCard.IsVisible = true;
        MenuCineCard.IsVisible = true;
        MenuPatrocinadoresCard.IsVisible = true;

    }

    private void UpdateCardColors()
    {
        MenuClimaCard.BackgroundColor = SwitchClima.IsToggled ? Colors.LightGreen : Colors.White;
        MenuCotizacionesCard.BackgroundColor = SwitchCotizaciones.IsToggled ? Colors.LightGreen : Colors.White;
        MenuCineCard.BackgroundColor = SwitchCine.IsToggled ? Colors.LightGreen : Colors.White;
        MenuPatrocinadoresCard.BackgroundColor = SwitchPatrocinadores.IsToggled ? Colors.LightGreen : Colors.White;

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

    private void TapPatrocinadores_Tapped(object sender, TappedEventArgs e)
    {
        
            SwitchPatrocinadores.IsToggled = !SwitchPatrocinadores.IsToggled;
            Preferences.Set("MenuPatrocinadoresVisible", SwitchPatrocinadores.IsToggled);
            UpdateCardColors();
            (Shell.Current as AppShell)?.ConfigurarMenu();
        
    }
}

