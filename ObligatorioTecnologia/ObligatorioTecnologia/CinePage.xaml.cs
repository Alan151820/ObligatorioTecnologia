using CommunityToolkit.Maui.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObligatorioTecnologia.Clases;
using RestSharp;
using System.Collections.ObjectModel;

namespace ObligatorioTecnologia;

public partial class CinePage : ContentPage
{
    public CinePage()
    {

        InitializeComponent();
        consumirAPI(-1, false);
        GetGenrosAsync();



    }
    private async void consumirAPI(int genero, bool fecha)
    {
        var options = new RestClientOptions("https://api.themoviedb.org")
        {
            MaxTimeout = -1,
        };
        var client = new RestClient(options);

        DateTime fechaSeleccioanda = DateTime.Now;
        fechaSeleccioanda = FechaPicker.Date;
        fechaSeleccioanda.ToString("yyyy-MM-dd");
        string url = "/3/discover/movie?include_adult=false&include_video=true&language=en-US&page=1&sort_by=popularity.desc&primary_release_date.gte=" + fechaSeleccioanda;

        if (genero >= 0)
        {
            url += "&with_genres=" + genero;
        }



        var request = new RestRequest(url, Method.Get);
        request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJkNTc4OTE3ZDAxMmNjNjQyNDk3ZDMxYWFhOWE1MGZmZiIsIm5iZiI6MTc1NDYwODc4MC4xMjk5OTk5LCJzdWIiOiI2ODk1MzQ4YzUwNDE4MmQ3ODEyOGE3ZGEiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.DAA8edMfJv0vmHQdvFTIuMoxuLSX4gzVpK8p0e7RPTM");

        RestResponse response = await client.ExecuteAsync(request);
        var json = response.Content;

        var peliculas = JsonConvert.DeserializeObject<Peliculas>(json);

        string baseUrl = "https://image.tmdb.org/t/p/w500";

        var lista = peliculas.Results.Select(p => new
        {
            Title = p.Title,
            PosterUrl = baseUrl + p.PosterPath,
            Descripcion = p.Overview,
            Fecha = p.ReleaseDate,
            Id = p.Id
        }).ToList();

        PeliculasCollection.ItemsSource = lista;
    }


    private void OnPeliculaTapped(object sender, TappedEventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is not null)
        {
            var pelicula = frame.BindingContext;
            this.ShowPopup(new ModalPelicula(pelicula));
        }
    }
    Genre generoFiltro;

    public async void GetGenrosAsync()
    {
        var options = new RestClientOptions("https://api.themoviedb.org")
        {
            MaxTimeout = -1,
        };
        var client = new RestClient(options);
        var request = new RestRequest("/3/genre/movie/list?api_key=d578917d012cc642497d31aaa9a50fff&language=es-ES", Method.Get);
        request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJkNTc4OTE3ZDAxMmNjNjQyNDk3ZDMxYWFhOWE1MGZmZiIsIm5iZiI6MTc1NDYwODc4MC4xMjk5OTk5LCJzdWIiOiI2ODk1MzQ4YzUwNDE4MmQ3ODEyOGE3ZGEiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.DAA8edMfJv0vmHQdvFTIuMoxuLSX4gzVpK8p0e7RPTM");
        RestResponse response = await client.ExecuteAsync(request);


        var genreResponse = JsonConvert.DeserializeObject<Genero>(response.Content);
        var genres = genreResponse.Genres;
        GenrePicker.ItemsSource = genres;


    }


    private void Filtrar_OnClicked(object sender, EventArgs e)
    {
        generoFiltro = GenrePicker.SelectedItem as Genre;
        if (generoFiltro != null)
        {
            consumirAPI((int)generoFiltro.Id, true);
        }
        else
        {
            consumirAPI(-1, true);

        }
    }

    private void BorrarFiltro_OnClicked(object sender, EventArgs e)
    {
        GenrePicker.SelectedIndex = -1;
        FechaPicker.Date = DateTime.Now;
        consumirAPI(-1, false);

    }
}