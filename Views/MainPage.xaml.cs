using PokeDexMVVM.Models;
using PokeDexMVVM.ViewModels;
using System.Collections.Generic;       // se necesita para Dictionary 

namespace PokeDexMVVM.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            // Constructor que recibe el View model por el DI y lo conecta a la pagina para que XAML pueda mostrar los datos
            InitializeComponent();
            BindingContext = viewModel;
        }

        // Solo carga los pokemons la primera vez; si ya hay datos, no vuelve a pedirlos a la API
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainViewModel vm)
            {
                if (vm.ListaPokemons.Count == 0)
                    vm.CargarPokemonsCommand.Execute(null);
                else
                    _ = vm.ActualizarEstadosAsync();
            }
        }

        // Se ejecuta al tocar la imagen/nombre del pokemon, navega al detalle
        private async void OnPokemonTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is PokemonResult seleccionado)
            {
                var parametros = new Dictionary<string, object>
                {
                    { "urlPokemon", seleccionado.Url },
                    { "nombrePokemon", seleccionado.Nombre }
                };

                await Shell.Current.GoToAsync(nameof(DetailPage), parametros);
            }
        }
    }
}


