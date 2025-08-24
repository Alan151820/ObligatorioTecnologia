using Microsoft.Maui.Devices.Sensors; // Geocoding
using ObligatorioTecnologia.Data;
using ObligatorioTecnologia.Modelo;

namespace ObligatorioTecnologia;

public partial class SponsorFormPage : ContentPage
{
	private readonly SponsorRepository _repo;
	public SponsorFormPage(SponsorRepository repo)
	{
        InitializeComponent();
        _repo = repo;
    }
    
	private async void OnGuardar(object sender, EventArgs e)
	{
		var s = new Sponsor 
		{
			Nombre = NombreEntry.Text?.Trim() ?? "",
			Descripcion = DescripcionEntry.Text?.Trim() ?? "",
			Direccion = DireccionEntry.Text?.Trim() ?? "",
			PlanID = 0,
			PlanPagado = false
        };

        if (!string.IsNullOrWhiteSpace(s.Direccion))
        {
            var locs = await Geocoding.GetLocationsAsync(s.Direccion);
            var l = locs?.FirstOrDefault();
            if (l != null)
            {
                s.Latitud = l.Latitude;
                s.Longitud = l.Longitude;
            }
        }

        await _repo.SaveAsync(s);
        await DisplayAlert("OK", "Patrocinador guardado", "Cerrar");
        await Shell.Current.GoToAsync("..");
    }
}