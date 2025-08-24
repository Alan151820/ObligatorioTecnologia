using ObligatorioTecnologia.Data;
using ObligatorioTecnologia.Modelo;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
	

namespace ObligatorioTecnologia;

public partial class SponsorsPage : ContentPage
{
	private readonly SponsorRepository _repo;
	public SponsorsPage(SponsorRepository repo)
	{
        InitializeComponent();
        _repo = repo;
    }
    
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		SponsorsList.ItemsSource = await _repo.GetAllAsync();
    }

    private async void OnNuevo(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SponsorFormPage));
    }

    private async void OnPlanes(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PlansPage));      
    }

    private async void OnMapa(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SponsorsMapPage));  
    }
    
    private async void OnSelChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Sponsor s)
            await Shell.Current.GoToAsync($"{nameof(SponsorDetailPage)}?sid={s.Id}");
        ((CollectionView)sender).SelectedItem = null;
    }

}