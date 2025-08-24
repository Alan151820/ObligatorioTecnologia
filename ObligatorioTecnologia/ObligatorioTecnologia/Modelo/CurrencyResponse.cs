using System.Text.Json.Serialization;
using System.Text.Json;

namespace ObligatorioTecnologia.Models
{
    public class CurrencyResponse
    {
        [JsonPropertyName("éxito")]
        public bool Exito { get; set; }

        [JsonPropertyName("términos")]
        public string Terminos { get; set; }

        [JsonPropertyName("privacidad")]
        public string Privacidad { get; set; }

        [JsonPropertyName("marca_de_tiempo")]
        public long MarcaDeTiempo { get; set; }

        [JsonPropertyName("fuente")]
        public string Fuente { get; set; }

        [JsonPropertyName("citas")]
        public Dictionary<string, decimal> Citas { get; set; }

        public async Task<CurrencyResponse> CargarDatosMonedas()
        {
            try
            {
                string apiKey = "126fd50e823ed9d2ef76d90b0a3fe9ab";
                string url = $"https://api.currencylayer.com/live?access_key={apiKey}&currencies=EUR,GBP,UYU,BRL&source=USD&format=1";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    var data = JsonSerializer.Deserialize<CurrencyResponse>(response);
                    return data;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    };

   
}
