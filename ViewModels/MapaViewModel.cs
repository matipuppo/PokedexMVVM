using PokeDexMVVM.Models;
using PokeDexMVVM.Services;
using Microsoft.Maui.Devices.Sensors;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PokeDexMVVM.ViewModels
{
    // ViewModel de la pantalla del mapa: obtiene la ubicación real del usuario por GPS
    // y genera 6 Pokémon aleatorios en coordenadas cercanas con su distancia calculada.
    public class MapaViewModel : BaseViewModel
    {
        // Servicio que accede al sensor GPS del dispositivo y maneja el permiso de ubicación
        private readonly LocationService servicioUbicacion;

        // Servicio HTTP que consulta la PokéAPI para obtener nombre e imagen de cada Pokémon
        private readonly PokemonService servicioPokemon;

        // Generador de números aleatorios para elegir Pokémon y calcular coordenadas al azar
        private readonly Random random = new();

        // Lista observable de Pokémon cercanos — la UI se actualiza automáticamente cuando cambia
        public ObservableCollection<PokemonEnMapa> PokemonsCercanos { get; } = new();

        // Mensaje de estado que se muestra en la pantalla (cargando, error, resultado)
        private string mensajeEstado;
        public string MensajeEstado
        {
            get => mensajeEstado;
            set => SetProperty(ref mensajeEstado, value);
        }

        // Latitud real del usuario obtenida por GPS — se muestra en pantalla
        private double latitudUsuario;
        public double LatitudUsuario
        {
            get => latitudUsuario;
            set => SetProperty(ref latitudUsuario, value);
        }

        // Longitud real del usuario obtenida por GPS — se muestra en pantalla
        private double longitudUsuario;
        public double LongitudUsuario
        {
            get => longitudUsuario;
            set => SetProperty(ref longitudUsuario, value);
        }

        // Indica si la ubicación ya fue obtenida — controla la visibilidad de coordenadas en la UI
        private bool ubicacionObtenida;
        public bool UbicacionObtenida
        {
            get => ubicacionObtenida;
            set => SetProperty(ref ubicacionObtenida, value);
        }

        // Comando que dispara la búsqueda de Pokémon cercanos — se enlaza al botón en la vista
        public ICommand BuscarPokemonsCommand { get; }

        // Constructor: recibe ambos servicios por inyección de dependencias (DI)
        public MapaViewModel(LocationService servicioUbicacion, PokemonService servicioPokemon)
        {
            this.servicioUbicacion = servicioUbicacion;
            this.servicioPokemon = servicioPokemon;
            BuscarPokemonsCommand = new Command(async () => await BuscarPokemonsCercanosAsync());
        }

        // Método principal: obtiene el GPS, busca Pokémon en la API y los ubica en coordenadas cercanas
        public async Task BuscarPokemonsCercanosAsync()
        {
            try
            {
                MensajeEstado = "Obteniendo tu ubicación...";
                UbicacionObtenida = false;

                // Pide permiso de ubicación en tiempo de ejecución y obtiene las coordenadas reales
                var ubicacion = await servicioUbicacion.ObtenerUbicacionActualAsync();
                LatitudUsuario = ubicacion.Latitude;
                LongitudUsuario = ubicacion.Longitude;
                UbicacionObtenida = true;

                MensajeEstado = "Buscando Pokémon cercanos...";

                // Obtiene los primeros 151 Pokémon de la PokéAPI
                var lista = await servicioPokemon.ObtenerListaPokemon(151);
                PokemonsCercanos.Clear();

                // HashSet para evitar que aparezca el mismo Pokémon dos veces
                var usados = new HashSet<string>();
                int cantidad = 6;
                int intentos = 0;

                // Elige 6 Pokémon distintos al azar (con tope de 50 intentos para evitar loop infinito)
                while (PokemonsCercanos.Count < cantidad && intentos < 50)
                {
                    intentos++;
                    var pokemon = lista.Resultados[random.Next(lista.Resultados.Count)];

                    // Si este Pokémon ya fue elegido, salta al siguiente intento
                    if (usados.Contains(pokemon.Nombre)) continue;
                    usados.Add(pokemon.Nombre);

                    // Obtiene la imagen del Pokémon desde la API
                    var detalle = await servicioPokemon.ObtenerDetallePokemonAsync(pokemon.Url);

                    // Genera coordenadas aleatorias dentro de ~550m alrededor del usuario
                    // Dividir por 100 desplaza aproximadamente ±0.005 grados ≈ ±550 metros
                    double lat = ubicacion.Latitude + (random.NextDouble() - 0.5) / 100.0;
                    double lon = ubicacion.Longitude + (random.NextDouble() - 0.5) / 100.0;

                    // Calcula la distancia real en kilómetros entre el usuario y el Pokémon
                    double distanciaKm = Location.CalculateDistance(
                        ubicacion.Latitude, ubicacion.Longitude, lat, lon, DistanceUnits.Kilometers);

                    PokemonsCercanos.Add(new PokemonEnMapa
                    {
                        Nombre = pokemon.Nombre,
                        Imagen = detalle.Sprites.FrontDefault,
                        Latitud = lat,
                        Longitud = lon,
                        DistanciaMetros = distanciaKm * 1000
                    });
                }

                // Ordena la lista de más cercano a más lejano antes de mostrarla
                var ordenados = PokemonsCercanos.OrderBy(p => p.DistanciaMetros).ToList();
                PokemonsCercanos.Clear();
                foreach (var p in ordenados)
                    PokemonsCercanos.Add(p);

                MensajeEstado = $"Se encontraron {PokemonsCercanos.Count} Pokémon cerca tuyo.";
            }
            catch (Exception ex)
            {
                // Muestra el error en pantalla (permiso denegado, sin GPS, sin internet, etc.)
                MensajeEstado = ex.Message;
                UbicacionObtenida = false;
            }
        }
    }
}
