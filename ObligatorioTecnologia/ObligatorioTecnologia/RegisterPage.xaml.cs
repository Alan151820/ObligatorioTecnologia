using ObligatorioTecnologia.Clases;

namespace ObligatorioTecnologia;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private void BackLogin_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void RegisterUser_Clicked(object sender, EventArgs e)
    {
        string nombre = NombreEntry.Text.Trim();
        string email = EmailEntry.Text.Trim();
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        ErrorLabel.IsVisible = false;

        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ErrorLabel.Text = "Todos los campos son obligatorios.";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (password != confirmPassword)
        {
            ErrorLabel.Text = "Las contraseñas no coinciden.";
            ErrorLabel.IsVisible = true;
            return;
        }

        var existingUser = await App.UsuarioDB.GetUsuarioByEmailAsync(email);
        if (existingUser != null)
        {
            ErrorLabel.Text = "El email ya está registrado.";
            ErrorLabel.IsVisible = true;
            return;
        }

        var nuevoUsuario = new Usuario
        {
            Nombre = nombre,
            Email = email,
            Password = password,
            Image = "avatardefault.png"

        };

        await App.UsuarioDB.SaveUsuarioAsync(nuevoUsuario);

        await Navigation.PopAsync();
    }
}