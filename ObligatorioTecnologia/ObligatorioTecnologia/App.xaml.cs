using ObligatorioTecnologia.Clases;

namespace ObligatorioTecnologia
{
    public partial class App : Application
    {
        public static UsuarioDatabase UsuarioDB { get; private set; }
        public App(SponsorsPage sponsorsPage)
        {
            InitializeComponent();
            MainPage = new NavigationPage(sponsorsPage);
            Preferences.Remove("UsuarioEmail");
            Preferences.Remove("UsuarioNombre");
            Preferences.Remove("UsuarioImagen");
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "usuarios.db3");
            UsuarioDB = new UsuarioDatabase(dbPath);

            MainPage = new AppShell();


        }
    }
}
