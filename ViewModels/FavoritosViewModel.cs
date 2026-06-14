using PokeDexMVVM.Models;
using PokeDexMVVM.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace PokeDexMVVM.ViewModels
{
    // ViewModel QUe maneja la logica de la pantalla de favoritos
    public class FavoritosViewModel : BaseViewModel
    {
        // Repositorio local para acceder a la bd
        private readonly IPokemonLocalRepository repositorio;

        // Lista de pokemons favoritos que se muestra en la pantalla
        public ObservableCollection<PokemonFavorito> Favoritos { get; set; } = new();

        // Mensaje de estado para mostrar en la UI
        private string mensajeEstado;
        public string MensajEstado
        {
            get => mensajeEstado;
            set => SetProperty(ref mensajeEstado, value);
        }

        // Comando para eliminar un favorito
        public ICommand EliminarFavoritoCommand { get; }

        // Constructor que recibe el repositorio por DI
        public FavoritosViewModel(IPokemonLocalRepository repositorio)
        {
            this.repositorio = repositorio;
            EliminarFavoritoCommand = new Command<PokemonFavorito>(async(pokemon) => await EliminarFavoritosAsync(pokemon));
        }

        //Carga los favoritos desde la bd
        public async Task CargarFavoritosAsync()
        {
            try
            {
                mensajeEstado = "Cargando Favoritos...";
                var lista = await repositorio.ObtenerFavoritosAsync();
                Favoritos.Clear();
                foreach (var pokemon in lista)
                    Favoritos.Add(pokemon);

                mensajeEstado = Favoritos.Count == 0 ? "No tenes Pokemons favoritos guardados." : string.Empty; 
            }
            catch(Exception ex) 
            {
                MensajEstado = $"Error al cargar favoritos: {ex.Message}";
            }
        }

        //Eliminar un pokemon de favoritos y recargar la lista
        private async Task EliminarFavoritosAsync(PokemonFavorito pokemon)
        {
            await repositorio.EliminarFavoritoAsync(pokemon.Id);
            await CargarFavoritosAsync();
        }
    }
}
