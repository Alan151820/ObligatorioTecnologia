using Newtonsoft.Json;
using ObligatorioTecnologia.Modelo;
using System.Linq;


namespace ObligatorioTecnologia;


public partial class ClimaPage : ContentPage
{
    public ClimaPage()
    {
        InitializeComponent();
        CargarClima();
    }

    private async void CargarClima()
    {
        var apiUrl = "https://api.openweathermap.org/data/2.5/forecast?lat=-34.9&lon=-54.94&appid=a54ba4791103121fb778a7e957021412&units=metric";
        var hoy = DateTime.Now.Date;

        using var cliente = new HttpClient();
        var resp = await cliente.GetAsync(apiUrl);

        if (!resp.IsSuccessStatusCode)
        {
            await DisplayAlert("Error", "No se pudo cargar el clima.", "OK");
            return;
        }

        var json = await resp.Content.ReadAsStringAsync();
        var datosClima = JsonConvert.DeserializeObject<WeatherResponse>(json);

        if (datosClima == null || datosClima.List == null || datosClima.List.Count == 0)
        {
            await DisplayAlert("Error", "Respuesta de clima vacía.", "OK");
            return;
        }

        lblCiudad.Text = $"📍 {datosClima.City.Nombre}";

        // Tomamos, por ejemplo, las próximas 6 entradas (cada una ~3hs)
        var pronosticos = datosClima.List
            .Take(6)
            .Select(f => new PronosticoHora
            {
                Hora = DateTime.Parse(f.FechaTexto).ToString("HH:mm"),
                TempMax = f.Main.TempMax,
                TempMin = f.Main.TempMin,
                IconoUrl = $"https://openweathermap.org/img/wn/{f.Weather[0].Icono}@2x.png",
                Descripcion = f.Weather[0].Descripcion   
            }).ToList();

        PronosticoHoras.ItemsSource = pronosticos;

        var pronosticosDias = datosClima.List
            .GroupBy(f => DateTime.Parse(f.FechaTexto).Date)
            .Where(g => g.Key > hoy)       // siguientes días (excluye hoy)
            .OrderBy(g => g.Key)
            .Take(5)
            .Select(g =>
            {
                // Máx/Mín del día
                var max = g.Max(x => x.Main.TempMax);
                var min = g.Min(x => x.Main.TempMin);

                // Icono representativo: el más frecuente en el día
                var icono = g
                    .Select(x => x.Weather[0].Icono)
                    .GroupBy(i => i)
                    .OrderByDescending(gr => gr.Count())
                    .First().Key;

                return new PronosticoDia
                {
                    Dia = g.Key.ToString("ddd").ToUpper(),
                    TempMax = max,
                    TempMin = min,
                    IconoUrl = $"https://openweathermap.org/img/wn/{icono}@2x.png"

                };
            }).ToList();

        PronosticoDias.ItemsSource = pronosticosDias;
    }

}