namespace ObligatorioTecnologia;

public partial class EditUser : ContentPage
{
	public EditUser()
	{
		InitializeComponent();
        CargarDatosUsuario();

    }

    private async void CargarDesdeArchivoImg_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Selecciona tu nueva foto de perfil",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            var newFile = Path.Combine(FileSystem.AppDataDirectory, Path.GetFileName(result.FullPath));
            File.Copy(result.FullPath, newFile, true);
            UsuarioImagen.Source = ImageSource.FromFile(newFile);

            var email = Preferences.Get("UsuarioEmail", "");
            if (string.IsNullOrEmpty(email))
                return;

            var usuario = await App.UsuarioDB.GetUsuarioByEmailAsync(email);

            if (usuario != null)
            {
                usuario.Image = newFile;

                await App.UsuarioDB.UpdateUsuarioAsync(usuario);

                if (Application.Current.MainPage is AppShell shell)
                    await shell.CargarUsuarioActivo();
            }
        }
    }
    private async void CargarDatosUsuario()
    {
        var email = Preferences.Get("UsuarioEmail", "");
        if (string.IsNullOrEmpty(email))
            return;

        var usuario = await App.UsuarioDB.GetUsuarioByEmailAsync(email);

        if (usuario != null)
        {
            NombreEntry.Text = usuario.Nombre;
            EmailEntry.Text = usuario.Email;

            if (!string.IsNullOrEmpty(usuario.Image) && File.Exists(usuario.Image))
                UsuarioImagen.Source = ImageSource.FromFile(usuario.Image);
            else
                UsuarioImagen.Source = "avatar_default.png";
        }
    }

    private async void BtnGuardar_Clicked(object sender, EventArgs e)
    {
        var email = Preferences.Get("UsuarioEmail", "");
        if (string.IsNullOrEmpty(email))
        {
            await DisplayAlert("Error", "No se encontró la sesión del usuario.", "OK");
            return;
        }

        var usuario = await App.UsuarioDB.GetUsuarioByEmailAsync(email);
        if (usuario != null)
        {
            string nuevoNombre = NombreEntry.Text;

            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                await DisplayAlert("Error", "El nombre no puede estar vacío.", "OK");
                return;
            }

            usuario.Nombre = nuevoNombre;
            await App.UsuarioDB.UpdateUsuarioAsync(usuario);

            // Refrescar UI en AppShell
            if (Application.Current.MainPage is AppShell shell)
                await shell.CargarUsuarioActivo();

        }

        await Navigation.PopAsync();
    }

    private async void TomarFoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            var foto = await MediaPicker.CapturePhotoAsync();
            if (foto != null)
            {
                string ruta = foto.FullPath;

                await DisplayAlert("VALIDO", "FOTO REALIZADA CON EXITO", "ACEPTAR");
                UsuarioImagen.Source = ImageSource.FromFile(ruta);

                var email = Preferences.Get("UsuarioEmail", "");
                if (string.IsNullOrEmpty(email))
                    return;

                var usuario = await App.UsuarioDB.GetUsuarioByEmailAsync(email);

                if (usuario != null)
                {
                    usuario.Image = ruta;
                    await App.UsuarioDB.UpdateUsuarioAsync(usuario);

                    if (Application.Current.MainPage is AppShell shell)
                        await shell.CargarUsuarioActivo();
                }
            }
        }
        catch (Exception)
        {
            await DisplayAlert("ERROR", "ERROR AL ABRIR LA CAMARA", "CERRAR");
        }

    }
}