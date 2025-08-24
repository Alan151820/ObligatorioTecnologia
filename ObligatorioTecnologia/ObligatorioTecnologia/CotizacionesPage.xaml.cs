using ObligatorioTecnologia.Modelo;

namespace ObligatorioTecnologia;

public partial class CotizacionesPage : ContentPage
{
    public CotizacionesPage(CotizacionViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _ = vm.CargarAsync(); // carga inicial
    }
}