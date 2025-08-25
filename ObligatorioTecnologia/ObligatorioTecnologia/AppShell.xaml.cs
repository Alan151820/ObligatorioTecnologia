using ObligatorioTecnologia.Modelo;
using ObligatorioTecnologia.Data;

namespace ObligatorioTecnologia
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            CargarUsuarioActivo();
            ConfigurarMenu();

            Routing.RegisterRoute(nameof(SponsorFormPage), typeof(SponsorFormPage));
           

        }
        
        public Image UsuarioImagenPublic => UsuarioImagen;
        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }

        public async Task CargarUsuarioActivo()
        {
            var email = Preferences.Get("UsuarioEmail", string.Empty);

            if (!string.IsNullOrEmpty(email))
            {
                var usuario = await App.UsuarioDB.GetUsuarioByEmailAsync(email);

                if (usuario != null)
                {
                    UsuarioNombre.Text = usuario.Nombre.ToUpper();

                    if (usuario.Image.StartsWith("http"))
                        UsuarioImagen.Source = ImageSource.FromUri(new Uri(usuario.Image));
                    else if (File.Exists(usuario.Image))
                        UsuarioImagen.Source = ImageSource.FromFile(usuario.Image);
                    else
                        UsuarioImagen.Source = usuario.Image;

                    btnLogin.IsVisible = false;
                    btneditar.IsVisible = true;
                    btnCerrarSesion.IsVisible = true;
                }
            }
            else
            {
                UsuarioNombre.Text = "";
                UsuarioImagen.Source = "avatardefault.png";
                btneditar.IsVisible = false;
                btnLogin.IsVisible = true;
                btnCerrarSesion.IsVisible = false;
            }
        }



        private void btnEditar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditUser());
        }

        private async void btnCerrrarSesion_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("UsuarioEmail");
            Preferences.Remove("UsuarioNombre");
            Preferences.Remove("UsuarioImagen");
            await CargarUsuarioActivo();
        }

        public void ConfigurarMenu()
        {
            MenuClima.IsVisible = Preferences.Get("MenuClimaVisible", true);
            MenuCotizaciones.IsVisible = Preferences.Get("MenuCotizacionesVisible", true);
            MenuCine.IsVisible = Preferences.Get("MenuCineVisible", true);
            MenuPatrocinadores.IsVisible = Preferences.Get("MenuPatrocinadoresVisible", true);

        }

    }
}

