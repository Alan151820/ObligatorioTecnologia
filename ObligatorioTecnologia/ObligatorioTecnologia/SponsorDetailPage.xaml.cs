using ObligatorioTecnologia.Data;
using ObligatorioTecnologia.Modelo;
using Microsoft.Maui.Controls;

namespace ObligatorioTecnologia;

[QueryProperty(nameof(SponsorId), "sid")]
public partial class SponsorDetailPage : ContentPage
{
    private readonly SponsorRepository _sRepo;
    private readonly PostRepository _pRepo;
    private Sponsor? _sponsor;

    public int SponsorId { get; set; }

    public SponsorDetailPage(SponsorRepository sRepo, PostRepository pRepo)
    {
        InitializeComponent();
        _sRepo = sRepo; _pRepo = pRepo;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _sponsor = await _sRepo.GetAsync(SponsorId);
        if (_sponsor == null) { await DisplayAlert("Error", "Sponsor no encontrado", "OK"); await Shell.Current.GoToAsync(".."); return; }

        NombreLbl.Text = _sponsor.Nombre;
        PlanLbl.Text = _sponsor.PlanPagado ? "Plan activo" : "Plan pendiente";
        PostsList.ItemsSource = await _pRepo.GetBySponsorAsync(_sponsor.Id);
    }

    private async void OnPublicar(object sender, EventArgs e)
    {
        if (_sponsor == null) return;
        var texto = PostEditor.Text?.Trim();
        if (string.IsNullOrWhiteSpace(texto)) return;

        var post = new SponsorPost { SponsorId = _sponsor.Id, Titulo = "Novedad", Contenido = texto };
        await _pRepo.SaveAsync(post);

        PostEditor.Text = "";
        PostsList.ItemsSource = await _pRepo.GetBySponsorAsync(_sponsor.Id);
    }
}
