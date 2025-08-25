using ObligatorioTecnologia.Data;
using ObligatorioTecnologia.Modelo;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
	

namespace ObligatorioTecnologia;

public partial class SponsorsPage : ContentPage
{
    private readonly SponsorDb _db;

    public SponsorsPage(SponsorDb db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _db.InitAsync();
        SponsorsCollection.ItemsSource = await _db.GetAllAsync();
        SponsorsCollection.SelectedItem = null;
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SponsorFormPage(_db));
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection?.FirstOrDefault() is Sponsor s)
        {
            await Navigation.PushAsync(new SponsorFormPage(_db, s.Id));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}