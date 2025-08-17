namespace ObligatorioTecnologia;
using CommunityToolkit.Maui.Views;
using Newtonsoft.Json;
using ObligatorioTecnologia.Clases;
using RestSharp;

public partial class ModalPelicula : Popup
{
    dynamic peliculaActual;
    public ModalPelicula(Object pelicula)
    {
        InitializeComponent();

        peliculaActual = pelicula;
        dynamic p = pelicula;
        PosterImage.Source = p.PosterUrl;
        TituloLabel.Text = p.Title ?? "Título no disponible";
        DescripcionLabel.Text = p.Descripcion;
        FechaLabel.Text = p.Fecha.ToString("dd/MM/yyyy");
    }

    private async void Trailer_Clicked(object sender, EventArgs e)
    {
        var options = new RestClientOptions("https://api.themoviedb.org")
        {
            MaxTimeout = -1,
        };
        var client = new RestClient(options);
        var request = new RestRequest("/3/movie/" + peliculaActual.Id + "/videos?language=en-US", Method.Get);
        request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJkNTc4OTE3ZDAxMmNjNjQyNDk3ZDMxYWFhOWE1MGZmZiIsIm5iZiI6MTc1NDYwODc4MC4xMjk5OTk5LCJzdWIiOiI2ODk1MzQ4YzUwNDE4MmQ3ODEyOGE3ZGEiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.DAA8edMfJv0vmHQdvFTIuMoxuLSX4gzVpK8p0e7RPTM");
        RestResponse response = await client.ExecuteAsync(request);
        var json = response.Content;

        var video = JsonConvert.DeserializeObject<VideosTrailer>(json);

        var trailer = video.Results
    .FirstOrDefault(v => v.Type == "Trailer" && v.Site == "YouTube");

        var UrlTrailer = "https://www.youtube.com/watch?v=" + trailer.Key;
        await Microsoft.Maui.ApplicationModel.Launcher.OpenAsync(UrlTrailer);



    }
}