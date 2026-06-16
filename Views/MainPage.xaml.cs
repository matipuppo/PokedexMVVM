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

        // Se ejecuta automáticamente al entrar en MainPage
        protected override void OnAppearing()
        {
            // Llama al método base para asegurar que cualquier lógica de la clase base se ejecute
            base.OnAppearing();

            // Verifica si el BindingContext es del tipo MainViewModel
            if (BindingContext is MainViewModel vm)
            {
                // Ejecuta el comando para cargar los Pokémon desde la API
                vm.CargarPokemonsCommand.Execute(null);
            }
        }

        // Evento cuando se selecciona un Pokémon en la lista
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


