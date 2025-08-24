using Newtonsoft.Json;
using System.Collections.Generic;

namespace ObligatorioTecnologia.Modelo
{
    public class WeatherResponse
    {
        [JsonProperty("city")]
        public City City { get; set; }

        [JsonProperty("list")]
        public List<Forecast> List { get; set; }

        [JsonProperty("description")]
        public string Descripcion { get; set; }
    }

    public class City
    {
        [JsonProperty("name")]
        public string Nombre { get; set; }
    }

    public class Forecast
    {
        [JsonProperty("main")]
        public MainInfo Main { get; set; }

        [JsonProperty("weather")]
        public List<WeatherInfo> Weather { get; set; }

        [JsonProperty("dt_txt")]
        public string FechaTexto { get; set; }
    }

    public class MainInfo
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("temp_min")]
        public double TempMin { get; set; }

        [JsonProperty("temp_max")]
        public double TempMax { get; set; }
    }

    public class WeatherInfo
    {
        [JsonProperty("description")]
        public string Descripcion { get; set; }

        [JsonProperty("icon")]
        public string Icono { get; set; }
    }
}
