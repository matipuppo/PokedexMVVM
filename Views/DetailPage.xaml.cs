using PokeDexMVVM.ViewModels;

namespace PokeDexMVVM.Views
{
    public partial class DetailPage : ContentPage
    {
        public DetailPage(DetailViewModel viewModel)
        {
            // Constructor que recibe el ViewModel por DI y lo conecta a la pagina para que el XAML muestre los datos
            InitializeComponent();
            BindingContext = viewModel;

        }

        // Se ejecuta cuando la navegación completó y todos los QueryProperty ya están seteados
        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            if (BindingContext is DetailViewModel vm)
                await vm.CargarDetallePokemonAsync();
        }
    }
}


