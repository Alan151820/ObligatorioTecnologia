using ObligatorioTecnologia.Data;
using ObligatorioTecnologia.Modelo;
using System.Runtime.InteropServices.ObjectiveC;

namespace ObligatorioTecnologia;

public partial class PlansPage : ContentPage
{
	private readonly PlanRepository _plans;
	private readonly SponsorRepository _sponsors;
	

    public PlansPage(PlanRepository plans, SponsorRepository sponsors)
	{
        InitializeComponent();
        _plans = plans; _sponsors = sponsors;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Semilla de planes si está vacío
        var items = await _plans.GetAllAsync();
        if (items.Count == 0)
        {
            await _plans.SaveAsync(new Plan { Nombre = "Bronce", Precio = 5, Beneficios = "Mención mensual" });
            await _plans.SaveAsync(new Plan { Nombre = "Plata", Precio = 25, Beneficios = "Menciones destacadas + sorteos" });
            await _plans.SaveAsync(new Plan { Nombre = "Oro", Precio = 100, Beneficios = "Contenido exclusivo + prioridad" });
            items = await _plans.GetAllAsync();
        }
        PlansList.ItemsSource = items;
    }
    

    private async void OnSelChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Plan p)
        {
            // Ejemplo simple: asignar a primer sponsor (o pide selección)
            var sponsor = (await _sponsors.GetAllAsync()).FirstOrDefault();
            if (sponsor == null) { await DisplayAlert("Atención", "Cree un patrocinador primero.", "OK"); return; }

            sponsor.PlanID = p.Id;
            sponsor.PlanPagado = true; // “pago” simulado
            await _sponsors.SaveAsync(sponsor);
            await DisplayAlert("Listo", $"Plan '{p.Nombre}' asignado a {sponsor.Nombre}", "OK");
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
    
