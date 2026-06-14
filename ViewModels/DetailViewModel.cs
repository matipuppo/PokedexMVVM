using PokeDexMVVM.Models;           
using PokeDexMVVM.Services;
using System.Linq;                      // se usa para concatenar los tipos con LINQ

namespace PokeDexMVVM.ViewModels
{
    // El atributo QueryProperty permite recibir parámetros al navegar con Shell a esta página.
    // En este caso, recibimos la URL y el nombre del Pokémon.
    [QueryProperty(nameof(UrlPokemon), "urlPokemon")]
    [QueryProperty(nameof(NombrePokemon), "nombrePokemon")]
    public class DetailViewModel : BaseViewModel
    {
        // Instancia del servicio para obtener los detalles del Pokémon
        private readonly PokemonService servicioPokemon;

        // Propiedad para recibir la URL del Pokémon al navegar
        public string UrlPokemon { get; set; }

        // Propiedad para recibir el nombre del Pokémon al navegar. Al asignar el nombre, se carga el detalle.
        private string nombrePokemon;
        public string NombrePokemon
        {
            get => nombrePokemon;
            set
            {
                if (SetProperty(ref nombrePokemon, value))
                {
                    // Cuando se asigna el nombre, cargamos el detalle
                    _ = CargarDetallePokemonAsync();
                }
            }
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

        // Contructor que recibe el servicio por DI en lugar de crearlo directamente
        public DetailViewModel(PokemonService servicio)
        {
            servicioPokemon = servicio;
        }

        // Método para cargar el detalle del Pokémon usando la URL recibida. Se maneja cualquier error que pueda ocurrir.
        private async Task CargarDetallePokemonAsync()
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






