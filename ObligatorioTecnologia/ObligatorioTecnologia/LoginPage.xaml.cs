
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;

namespace ObligatorioTecnologia;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();

    }

    private void Register_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RegisterPage());
    }

    private async void LoginUse_Clicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text.Trim();
        string password = PasswordEntry.Text;

        ErrorLabel.IsVisible = false;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ErrorLabel.Text = "Por favor, complete todos los campos.";
            ErrorLabel.IsVisible = true;
            return;
        }

        var usuario = await App.UsuarioDB.GetUsuarioByEmailAsync(email);

        if (usuario == null)
        {
            ErrorLabel.Text = "Usuario no encontrado.";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (usuario.Password != password)
        {
            ErrorLabel.Text = "Contraseña incorrecta.";
            ErrorLabel.IsVisible = true;
            return;
        }
        Preferences.Set("UsuarioEmail", usuario.Email);
        Preferences.Set("UsuarioNombre", usuario.Nombre);
        Preferences.Set("UsuarioImagen", usuario.Image);

        if (Application.Current.MainPage is AppShell shell)
        {

            await shell.CargarUsuarioActivo();
        }

        Application.Current.MainPage = new AppShell();
    }

    private async void SesionConHuella_Clicked(object sender, EventArgs e)
    {
        try
        {
            var disponible = await CrossFingerprint.Current.IsAvailableAsync(true);
            if (!disponible)
            {
                await DisplayAlert("Error", "Huella no disponible o no registrada", "OK");
                return;
            }

            var request = new AuthenticationRequestConfiguration("Iniciar sesión", "Coloque su huella");
            var result = await CrossFingerprint.Current.AuthenticateAsync(request);

            if (result.Authenticated)
            {
                var email = "restellialan@gmail.com";
                var usuario = await App.UsuarioDB.GetUsuarioByEmailAsync(email);

                if (usuario != null)
                {
                    Preferences.Set("UsuarioEmail", usuario.Email);
                    Preferences.Set("UsuarioNombre", usuario.Nombre);
                    Preferences.Set("UsuarioImagen", usuario.Image);
                    Preferences.Set("SesionIniciada", true);

                    // Volvemos a MainPage
                    await Navigation.PopAsync();

                    // Actualizamos UI de AppShell
                    if (Application.Current.MainPage is AppShell shell)
                    {
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            await shell.CargarUsuarioActivo();
                        });
                    }

                }
                else
                {
                    await DisplayAlert("Error", "No se encontró el usuario en la base de datos", "Cerrar");
                }
            }
            else
            {
                await DisplayAlert("Error", "Huella no reconocida", "Cerrar");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al detectar huella: {ex.Message}", "Cerrar");
        }
    }

}


