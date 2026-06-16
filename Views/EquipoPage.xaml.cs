using PokeDexMVVM.ViewModels;

namespace PokeDexMVVM.Views
{
    public partial class EquipoPage : ContentPage
    {
        // Constructor que recibe el view model por DI
        public EquipoPage(EquipoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        // Se ejecuta automaticamente al entrar en equipo EquipoPage para recargar el equipo
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is EquipoViewModel viewModel)
            {
                await viewModel.CargarEquipoAsync();
            }
        }
    }
}
