using PokeDexMVVM.ViewModels;

namespace PokeDexMVVM.Views
{
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            // Inicializamos el componente visual y asignamos el ViewModel al BindingContext
            InitializeComponent();
            // Esto permite que los elementos en XAML se enlacen a la propiedades y comandos definidos en StartViewModel
            BindingContext = new StartViewModel();
        }
    }
}


