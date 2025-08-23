using CommunityToolkit.Maui.Converters;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;
using ObligatorioTecnologia.Clases;
using RestSharp;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace ObligatorioTecnologia
{


    public partial class MainPage : ContentPage
    {
        private const string API_KEY = "";
        //API FUNCIONAL PARA DEFENSA-->    pub_b96427e0ef374a2a8b6592650c9a02be


        public MainPage()
        {
            InitializeComponent();
            ConsumirAPINoticias("");
            ConsumirAPINoticiasDeportes();
        }

        private async void FiltrarNoticias_Clicked(object sender, EventArgs e)
        {
            string filtroUsuario = filtro.Text?.Trim() ?? "";
            await ConsumirAPINoticias(filtroUsuario);
        }

        private async Task ConsumirAPINoticias(string filtro)
        {
            try
            {
                var client = new RestClient("https://newsdata.io");

                RestRequest request;
                if (string.IsNullOrEmpty(filtro))
                {
                    request = new RestRequest($"/api/1/latest?apikey={API_KEY}&q=uruguay&language=es", Method.Get);
                }
                else
                {
                    request = new RestRequest($"/api/1/latest?apikey={API_KEY}&q={filtro}&language=es", Method.Get);
                }


                RestResponse response = await client.ExecuteAsync(request);
                var noticias = JsonConvert.DeserializeObject<Noticias>(response.Content);


                if (noticias?.Results != null && noticias.Results.Any())
                {

                    MostrarNoticias(noticias);
                }
                else
                {
                    await DisplayAlert("INFORMACIÓN", "No se encontraron noticias para este filtro", "OK");
                    NoticiasPrincipalesGrid.Clear();
                    NoticiasSecundariasLayout.Clear();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private void MostrarNoticias(Noticias noticias)
        {
            NoticiasPrincipalesGrid.Clear();
            NoticiasSecundariasLayout.Clear();



            var principales = noticias.Results.Take(2).ToList(); // Solo 2 principales

            for (int i = 0; i < principales.Count; i++)
            {
                var noticia = principales[i];

                // Imagen con borde redondeado y recorte
                var imagen = new Image
                {
                    Source = noticia.ImageUrl?.ToString(),
                    Aspect = Aspect.AspectFill,
                    HeightRequest = 180
                };

                var imagenFrame = new Frame
                {
                    Content = imagen,
                    CornerRadius = 12,
                    Padding = 0,
                    HasShadow = false,
                    IsClippedToBounds = true
                };

                // Título
                var titulo = new Label
                {
                    Text = noticia.Title,
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.DarkSlateBlue,
                    LineBreakMode = LineBreakMode.TailTruncation,
                    MaxLines = 2
                };

                // Descripción
                var descripcion = new Label
                {
                    Text = noticia.Description,
                    FontSize = 14,
                    TextColor = Colors.DarkGray,
                    LineBreakMode = LineBreakMode.TailTruncation,
                    MaxLines = 3
                };

                // Fecha
                var fecha = new Label
                {
                    Text = $"Publicado: {noticia.PubDate:dd/MM/yyyy HH:mm}",
                    FontSize = 11,
                    TextColor = Colors.Gray
                };
                var leerMasButton = new Button
                {
                    Text = "Leer más",
                    FontSize = 13,
                    BackgroundColor = Colors.Transparent,
                    TextColor = Colors.Blue,
                    Padding = new Thickness(0),
                    HorizontalOptions = LayoutOptions.Start
                };
                leerMasButton.Clicked += (_, _) =>
                {
                    Navigation.PushAsync(new WebPageDate("" + noticia.Link));
                };

                var contenidoStack = new VerticalStackLayout
                {
                    Padding = 10,
                    Spacing = 5,
                    Children = { titulo, descripcion, fecha, leerMasButton }
                };

                var frame = new Frame
                {
                    Content = new VerticalStackLayout
                    {
                        Children = { imagenFrame, contenidoStack },
                        Spacing = 0
                    },
                    CornerRadius = 18,
                    HasShadow = true,
                    Padding = 0,
                    BackgroundColor = Color.FromArgb("#F0F8FF"),
                    Shadow = new Shadow
                    {
                        Offset = new Point(2, 2),
                        Radius = 6
                    },
                    Margin = new Thickness(5)
                };

                NoticiasPrincipalesGrid.Add(frame, i, 0);
            }
            foreach (var noticia in noticias.Results.Skip(2).Take(10))
            {
                var frame = new Frame
                {
                    Padding = 5,
                    WidthRequest = 180,
                    HeightRequest = 300,
                    CornerRadius = 10,
                    BackgroundColor = Colors.WhiteSmoke,
                    HasShadow = true
                };

                var layout = new VerticalStackLayout
                {
                    Spacing = 5,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                layout.Add(new Image { Source = noticia.ImageUrl, HeightRequest = 100 });
                layout.Add(new Label { Text = noticia.Title, FontSize = 14, FontAttributes = FontAttributes.Bold });
                layout.Add(new Label { Text = $"{noticia.PubDate:dd/MM/yyyy}", FontSize = 12, TextColor = Colors.Gray });

                // Asignar contenido
                frame.Content = layout;

                frame.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        // Navegación interna
                        Navigation.PushAsync(new WebPageDate("" + noticia.Link));
                    })
                });
                // Agregar al layout principal
                NoticiasSecundariasLayout.Add(frame);


            }
        }



        private async Task ConsumirAPINoticiasDeportes()
        {
            try
            {
                var client = new RestClient("https://newsdata.io");

                RestRequest request;

                request = new RestRequest($"/api/1/latest?apikey={API_KEY}&q=deportes&language=es", Method.Get);



                RestResponse response = await client.ExecuteAsync(request);
                var noticiasDeporte = JsonConvert.DeserializeObject<Noticias>(response.Content);

                var client1 = new RestClient("https://newsdata.io");

                RestRequest requests;

                requests = new RestRequest($"/api/1/latest?apikey={API_KEY}&q=crypto&language=es", Method.Get);



                RestResponse responses = await client1.ExecuteAsync(requests);
                var noticiasCrypto = JsonConvert.DeserializeObject<Noticias>(responses.Content);
                MostrarNoticiasPorCategoria(noticiasDeporte, noticiasCrypto);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private void MostrarNoticiasPorCategoria(Noticias noticiasDeportes, Noticias noticiasCrypto)
        {
            BindingContext = new
            {
                SeccionesNoticias = new List<object>
             {
                new
             {
                Titulo = "Noticias de Deportes",
                Noticias = noticiasDeportes.Results.Take(5).ToList()
              },
              new
              {
                Titulo = "Noticias de Crypto",
                Noticias = noticiasCrypto.Results.Take(5).ToList()
              }
    }
            };
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.BindingContext is NoticiaResult noticia && noticia.Link != null)
            {
                Navigation.PushAsync(new WebPageDate("" + noticia.Link));
            }
        }

        private void Buscaminas_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WebPageDate("https://www.google.com/search?si=AMgyJEvpnN6BvoKq56S10zdfipJHAM-gSWeNnoY-Rdjqw7STMkOcrj0wmhE1llmkea7VsMzYzsASyvPJ7i390kY9U0ABbcXxNQ==&biw=1366&bih=633&dpr=1"));

        }

        private void Solitario_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WebPageDate("https://www.google.com/search?si=AMgyJEvpa4k8B71VZdM2h7jC39XnbU8Vygzk2OmikLuvyNJBE3wbXuDClEilikHdECdFu9wHk4hovTsvoH8kTOae3wT9k9OeeQ==&biw=1366&bih=633&dpr=1"));

        }

        private void TaTeTi_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WebPageDate("https://www.google.com/search?si=AMgyJEtX_kzicVpodWYNi6on18nBP5HcFixmwuMuqi8WfNNW5bKi7ghVXe5RgOAgQ1TVAJyCOkXgI-UVGex2aavpwFq6Vqsnkg==&biw=1366&bih=633&dpr=1"));


        }

        private void Snake_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WebPageDate("https://www.google.com/search?si=AMgyJEvEToJYX5XMR5nMh1aSGR-1FSwfvZQ5zM2TThgX-ihTbZSoZ0tyvHqvaDPBQnquV5feoCy6C6xsBFkwJKgKJ6bE_AdUqA==&biw=1366&bih=633&dpr=1"));

        }
    }
}

