using PokeDexMVVM.Views;

namespace PokeDexMVVM
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            // Inicializa los componentes definidos en AppShell.xaml
            InitializeComponent();
            // Registra las rutas para la navegación dentro de la aplicación
            Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));
            Routing.RegisterRoute(nameof(Views.MainPage), typeof(Views.MainPage));
            Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
        }
    }
}


