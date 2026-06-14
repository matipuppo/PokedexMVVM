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
        private async void OnPokemonSeleccionado(object sender, SelectionChangedEventArgs e)
        {
            //Obtiene el primer elemento seleccionado en la lista y verifica si es un PokemonResult
            if (e.CurrentSelection.FirstOrDefault() is PokemonResult seleccionado)
            {
                // Crea un diccionario de parámetros para pasar a la página de detalles
                var parametros = new Dictionary<string, object>
                {
                    { "urlPokemon", seleccionado.Url }, // URL del Pokémon seleccionado
                    { "nombrePokemon", seleccionado.Nombre } // Nombre del Pokémon seleccionado
                };

                // Navega a la página de detalles, pasando los parámetros
                await Shell.Current.GoToAsync(nameof(DetailPage), parametros);
            }
        }
    }
}


