using PokeDexMVVM.ViewModels;

namespace PokeDexMVVM.Views
{
    // Muestra la lista de pokemons guuardados como favoritos
    public partial class FavoritosPage : ContentPage
    {
        // Constructor que recibe al ViewModel por DI y lo conecta a la pagina
        public FavoritosPage(FavoritosViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        // Carga los favoritos cada vez que se entra a la pantalla
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is FavoritosViewModel viewModel)
                await viewModel.CargarFavoritosAsync();
        }
    }
}
