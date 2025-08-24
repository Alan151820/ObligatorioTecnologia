using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ObligatorioTecnologia.Services;

namespace ObligatorioTecnologia.Modelo;

public class CotizacionViewModel : INotifyPropertyChanged
{
    private readonly CurrencyLayerClient _client;

    public CotizacionViewModel(CurrencyLayerClient client)
    {
        _client = client;
        CargarCommand = new Command(async () => await CargarAsync());
        RefrescarCommand = new Command(async () => await CargarAsync());
    }

    private bool _isBusy;
    private string? _error;
    private DateTime? _ultimaActualizacion;
    private decimal _uyuPorUsd;
    private decimal _uyuPorEur;
    private decimal _uyuPorBrl;

    public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }
    public string? Error { get => _error; set { _error = value; OnPropertyChanged(); } }
    public DateTime? UltimaActualizacion { get => _ultimaActualizacion; set { _ultimaActualizacion = value; OnPropertyChanged(); } }
    public decimal UyuPorUsd { get => _uyuPorUsd; set { _uyuPorUsd = value; OnPropertyChanged(); } }
    public decimal UyuPorEur { get => _uyuPorEur; set { _uyuPorEur = value; OnPropertyChanged(); } }
    public decimal UyuPorBrl { get => _uyuPorBrl; set { _uyuPorBrl = value; OnPropertyChanged(); } }

    public ICommand CargarCommand { get; }
    public ICommand RefrescarCommand { get; }

    public async Task CargarAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            Error = null;

            var res = await _client.GetRatesAsync();

            UyuPorUsd = Math.Round(res.PesosPorUSD, 2);
            UyuPorEur = Math.Round(res.PesosPorEUR, 2);
            UyuPorBrl = Math.Round(res.PesosPorBRL, 2);
            UltimaActualizacion = res.Timestamp;
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}