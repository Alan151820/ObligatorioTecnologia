using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ObligatorioTecnologia.Services;
public class CurrencyLayerClient
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public CurrencyLayerClient(HttpClient http, string apiKey)
    {
        _http = http;
        _apiKey = apiKey;
    }

    public async Task<RatesResult> GetRatesAsync(CancellationToken ct = default)
    {
        // Free plan: solo HTTP (si tienes paid, cambia a https)
        var url = $"http://api.currencylayer.com/live?access_key={_apiKey}&currencies=UYU,EUR,BRL&source=USD";

        using var resp = await _http.GetAsync(url, ct);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync(ct);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var data = JsonSerializer.Deserialize<CurrencyLayerResponse>(json, options)
                   ?? throw new Exception("Respuesta vacía de Currencylayer.");

        if (!data.Success)
            throw new Exception(data.Error?.Info ?? "Error desconocido de Currencylayer.");

        // Quotes vienen en base USD: USDUYU, USDEUR, USDBRL
        // Queremos: UYU por USD (USDUYU),
        //           UYU por EUR  = USDUYU / USDEUR
        //           UYU por BRL  = USDUYU / USDBRL
        var quotes = data.Quotes ?? throw new Exception("No llegaron quotes.");

        decimal usdUyu = Get(quotes, "USDUYU");
        decimal usdEur = Get(quotes, "USDEUR");
        decimal usdBrl = Get(quotes, "USDBRL");

        if (usdEur == 0 || usdBrl == 0)
            throw new Exception("Valores inválidos en las cotizaciones.");

        return new RatesResult
        {
            Timestamp = DateTimeOffset.FromUnixTimeSeconds(data.Timestamp).LocalDateTime,
            PesosPorUSD = usdUyu,
            PesosPorEUR = usdUyu / usdEur,
            PesosPorBRL = usdUyu / usdBrl,
            RawQuotes = quotes
        };

        static decimal Get(Dictionary<string, decimal> dict, string key)
            => dict.TryGetValue(key, out var v) ? v : throw new Exception($"Falta {key}.");
    }
}

public class CurrencyLayerResponse
{
    public bool Success { get; set; }
    public long Timestamp { get; set; }
    public string Source { get; set; } = "USD";
    public Dictionary<string, decimal>? Quotes { get; set; }
    public CurrencyLayerError? Error { get; set; }
}

public class CurrencyLayerError
{
    public int Code { get; set; }
    public string? Info { get; set; }
}

public class RatesResult
{
    public DateTime Timestamp { get; set; }
    public decimal PesosPorUSD { get; set; }
    public decimal PesosPorEUR { get; set; }
    public decimal PesosPorBRL { get; set; }
    public Dictionary<string, decimal>? RawQuotes { get; set; }
}
