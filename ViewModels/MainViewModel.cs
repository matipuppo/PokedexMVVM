using System.Collections.ObjectModel;       // se usa para ObservableCollection
using System.Threading.Tasks;               // se usa para Task
using System.Windows.Input;                 // se usa para ICommand
using PokeDexMVVM.Services;                 
using PokeDexMVVM.Models;
using System.Linq;
using PokeDexMVVM.Repositories;                          // se usa para filtrar con LINQ

namespace PokeDexMVVM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // Servicio para consumir la API
        private readonly PokemonService servicioPokemon;

        //Repositorio local para acceder a favoritos en la bd
        private readonly IPokemonLocalRepository repositorio;

        // Lista completa de pokmeons
        private ObservableCollection<PokemonResult> TodosLosPokemons { get; set; } = new();

        // Lista filtrada que se muestra en la UI
        public ObservableCollection<PokemonResult> ListaPokemons { get; set; } = new();

        // Mensaje de estado para mostrar en la UI (cargando, errores, etc.)
        private string mensajeEstado;
        public string MensajeEstado
        {
            get => mensajeEstado;
            set => SetProperty(ref mensajeEstado, value);
        }

        // Texto de búsqueda ingresado por el usuario
        private string textoBusqueda;
        public string TextoBusqueda
        {
            get => textoBusqueda;
            set
            {
                if (SetProperty(ref textoBusqueda, value))
                {
                    FiltrarPokemons(); // filtra automáticamente al escribir
                }
            }
        }

        // Comandos para la UI (para cargar y filtrar pokemones)
        public ICommand CargarPokemonsCommand { get; }
        public ICommand FiltrarPokemonsCommand { get; }
        public ICommand ToggleFavoritoCommand { get; }

        // Constructor recibe el servicio por DI en lugar de crearlo directamente
        public MainViewModel(PokemonService servicio, IPokemonLocalRepository repositorio)
        {
            servicioPokemon = servicio;
            this.repositorio = repositorio;
            CargarPokemonsCommand = new Command(async () => await CargarPokemonsAsync() );
            FiltrarPokemonsCommand = new Command(FiltrarPokemons);
            ToggleFavoritoCommand = new Command<PokemonResult>(async(pokemon) => await ToggleFavoritoAsync(pokemon));
        }

        // Método para cargar Pokémon desde la API
        private async Task CargarPokemonsAsync()
        {
            try
            {
                // Se muestra un mensaje de estado mientras se cargan los pokemones
                MensajeEstado = "Cargando pokemones...";

                // Se obtiene la lista de pokemones desde el servicio
                var lista = await servicioPokemon.ObtenerListaPokemon();

                // Se limpian las listas antes de agregar los nuevos datos
                TodosLosPokemons.Clear();
                ListaPokemons.Clear();

                // Recorremos cada pokemon obtenido para obtener su detalle y agregarlo a las listas
                foreach (var pokemon in lista.Resultados)
                {
                    // Se obtiene el detalle de cada pokemon para obtener su imagen
                    var detalle = await servicioPokemon.ObtenerDetallePokemonAsync(pokemon.Url);

                    // Se crea un nuevo objeto PokemonResult con el nombre, URL y la imagen del pokemon
                    var resultado = new PokemonResult
                    {
                        Nombre = pokemon.Nombre,
                        Url = pokemon.Url,
                        Imagen = detalle.Sprites.FrontDefault,
                        EsFavorito = await repositorio.EsFavoritoAsync(pokemon.Nombre)
                    };

                    // Se agrega el resultado a la lista completa y a la lista filtrada
                    TodosLosPokemons.Add(resultado);
                    ListaPokemons.Add(resultado);
                }

                // Se muestra un mensaje de estado indicando que los pokemones se cargaron exitosamente
                MensajeEstado = "Pokemones cargados exitosamente.";
            }
            catch (Exception ex)
            {
                // En caso de error, se muestra el mensaje de error en el estado
                MensajeEstado = ex.Message;
            }
        }

        // Metodo para agregar o quitar un pokemon de favoritos
        private async Task ToggleFavoritoAsync(PokemonResult pokemon)
        {
            if (pokemon.EsFavorito)
            {
                // Si ya es favorito, los busca por nombre y lo elimina
                var favoritos = await repositorio.ObtenerFavoritosAsync();
                var favorito = favoritos.FirstOrDefault(f => f.Nombre == pokemon.Nombre);
                if (favorito != null)
                    await repositorio.EliminarFavoritoAsync(favorito.Id);
            }
            else
            {
                // Si no es favorito, se agrega
                await repositorio.AgregarFavoritoAsync(new PokemonFavorito
                {
                    Nombre = pokemon.Nombre,
                    Url = pokemon.Url,
                    Imagen = pokemon.Imagen
                });
            }

            // Actualiaz el estado del icono en la UI
            pokemon.EsFavorito = !pokemon.EsFavorito;
            OnPropertyChanged(nameof(ListaPokemon));
        }

        // Método para filtrar según el texto de búsqueda
        private void FiltrarPokemons()
        {
            // Se limpia la lista filtrada antes de agregar los resultados filtrados
            ListaPokemons.Clear();

            // Si el texto de búsqueda está vacío, se muestran todos los pokemones; de lo contrario, se filtran por nombre
            var filtrados = string.IsNullOrWhiteSpace(TextoBusqueda)
                ? TodosLosPokemons
                : TodosLosPokemons.Where(p =>
                    p.Nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase));

            // Recorremos los pokemones filtrados y los agregamos a la lista que se muestra en la UI
            foreach (var p in filtrados)
                ListaPokemons.Add(p);
        }
    }
}


