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
    }
}


