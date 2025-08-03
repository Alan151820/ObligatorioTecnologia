namespace ObligatorioTecnologia
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Usuario usuario = null;

            //if (usuario == null)
            //{
            //    UsuarioNombre.Text = "No has iniciado sesión";
            //  UsuarioImagen.IsVisible = false;
            //    btnLogin.IsVisible = true;
            //}
            //else
            //{
            //    UsuarioNombre.Text = "Bienvenido/a " + usuario.Nombre;
            //    UsuarioImagen.Source = usuario.Imagen;
            //    btnLogin.IsVisible = false;
            //}
        }

        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }
    }
}
