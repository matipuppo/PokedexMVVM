using PokeDexMVVM.Models;
using PokeDexMVVM.Services;
using System.Linq;                      // se usa para concatenar los tipos con LINQ

namespace PokeDexMVVM.ViewModels
{
   
    public class DetailViewModel : BaseViewModel, IQueryAttributable
    {
        // Instancia del servicio para obtener los detalles del Pokémon
        private readonly PokemonService servicioPokemon;

        // Propiedad para recibir la URL del Pokémon al navegar
        private string urlPokemon;
        public string UrlPokemon
        {
            get => urlPokemon;
            set => SetProperty(ref urlPokemon, value);
        }

        // Propiedad para recibir el nombre del Pokémon al navegar
        private string nombrePokemon;
        public string NombrePokemon
        {
            get => nombrePokemon;
            set => SetProperty(ref nombrePokemon, value);
        }

        // Propiedad para almacenar el detalle del Pokémon obtenido del servicio
        private DetallePokemon detallePokemon;
        public DetallePokemon DetallePokemon
        {
            get => detallePokemon;
            set
            {
                if (SetProperty(ref detallePokemon, value))
                {
                    // Cuando se asigna el detalle, notificamos que el tipo concatenado también ha cambiado
                    OnPropertyChanged(nameof(TipoTexto));
                }
            }
        }

        // Propiedad calculada para mostrar el tipo concatenado
        public string TipoTexto
        {
            get
            {
                if (DetallePokemon?.Tipos == null || !DetallePokemon.Tipos.Any())
                    return string.Empty;

                return string.Join("/", DetallePokemon.Tipos.Select(t => t.Tipo.NombreEspañol));
            }
        }

        // Propiedad para mostrar mensajes de estado/errores en la UI
        private string mensajeEstado;
        public string MensajeEstado
        {
            get => mensajeEstado;
            set => SetProperty(ref mensajeEstado, value);
        }

        // Constructor que recibe el servicio por DI en lugar de crearlo directamente
        public DetailViewModel(PokemonService servicio)
        {
            servicioPokemon = servicio;
        }

        // MAUI llama este método con todos los parámetros de navegación ya seteados
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("urlPokemon", out var url))
                UrlPokemon = url?.ToString();
            if (query.TryGetValue("nombrePokemon", out var nombre))
                NombrePokemon = nombre?.ToString();

            _ = CargarDetallePokemonAsync();
        }

        // Método para cargar el detalle del Pokémon usando la URL recibida. Se maneja cualquier error que pueda ocurrir.
        public async Task CargarDetallePokemonAsync()
        {
            if (string.IsNullOrEmpty(UrlPokemon)) return;

            try
            {
                MensajeEstado = "Cargando detalles...";
                DetallePokemon = await servicioPokemon.ObtenerDetallePokemonAsync(UrlPokemon);
                MensajeEstado = "Detalles cargados correctamente.";
            }
            catch (Exception ex)
            {
                // El servicio ya devuelve un mensaje claro con número de error
                MensajeEstado = ex.Message;
            }
        }
    }
}






