using PokeDexMVVM.Views;

namespace PokeDexMVVM
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            // Inicializa los componentes definidos en AppShell.xaml
            InitializeComponent();


            // Solo se registran rutas de páginas que no son parte del TabBar
            Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
        }
    }
}


