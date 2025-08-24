using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using ObligatorioTecnologia.Data;
using ObligatorioTecnologia.Modelo;

namespace ObligatorioTecnologia;

public partial class SponsorsMapPage : ContentPage
{
    private readonly SponsorRepository _repo;

    public SponsorsMapPage(SponsorRepository repo)
    {
        InitializeComponent();
        _repo = repo;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarPins();
    }

    private async Task CargarPins()
    {
        Mapa.Pins.Clear();
        var sponsors = await _repo.GetAllAsync();
        foreach (var s in sponsors.Where(s => s.Latitud.HasValue && s.Longitud.HasValue))
        {
            var pin = new Pin
            {
                Label = s.Nombre,
                Address = s.Direccion,
                Location = new Location(s.Latitud.Value, s.Longitud.Value),
                Type = PinType.Place
            };
            Mapa.Pins.Add(pin);
        }

        // Centrar mapa si hay al menos uno
        var first = sponsors.FirstOrDefault(s => s.Latitud.HasValue && s.Longitud.HasValue);
        if (first != null)
            Mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(first.Latitud.Value, first.Longitud.Value), Distance.FromKilometers(5)));
    }

    private async void OnGuardarYMarcar(object sender, EventArgs e)
    {
        var dir = DireccionEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(dir)) { await DisplayAlert("Atención", "Ingrese una dirección", "OK"); return; }

        var locs = await Geocoding.GetLocationsAsync(dir);
        var l = locs?.FirstOrDefault();
        if (l == null) { await DisplayAlert("Ups", "No pude geocodificar la dirección", "OK"); return; }

        // Ejemplo: asignar esta ubicación al primer sponsor (o podrías elegir uno en UI)
        var sponsor = (await _repo.GetAllAsync()).FirstOrDefault();
        if (sponsor == null) { await DisplayAlert("Atención", "Cree un patrocinador primero", "OK"); return; }

        sponsor.Direccion = dir;
        sponsor.Latitud = l.Latitude;
        sponsor.Longitud = l.Longitude;
        await _repo.SaveAsync(sponsor);

        await CargarPins();
        await DisplayAlert("OK", "Dirección guardada y pin agregado", "Cerrar");
    }
}
