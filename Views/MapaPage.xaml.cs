using Microsoft.Maui.Controls.Maps;     // Control Map y Pin
using Microsoft.Maui.Devices.Sensors;   // Location (coordenadas)
using Microsoft.Maui.Maps;              // MapSpan y Distance
using PokeDexMVVM.Models;               // PokemonEnMapa
using PokeDexMVVM.ViewModels;
using Map = Microsoft.Maui.Controls.Maps.Map;   // Alias para evitar ambigüedad de "Map"

namespace PokeDexMVVM.Views
{
    public partial class MapaPage : ContentPage
    {
        // ViewModel inyectado por DI con la lógica de geolocalización
        private readonly MapaViewModel viewModel;

        // Control de mapa nativo (solo se crea en Android)
        private Map mapa;

        public MapaPage(MapaViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = viewModel;

            // El control Map solo funciona en Android
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                // Crea el mapa mostrando la ubicación del usuario y lo coloca en el contenedor
                mapa = new Map { IsShowingUser = true };
                MapContainer.Content = mapa;
            }
            else
            {
                // En Windows no hay mapa nativo: se oculta el contenedor
                MapContainer.HeightRequest = 0;
                MapContainer.IsVisible = false;
            }

            // Cada vez que el VM actualiza la lista de Pokémon, se redibujan los pines del mapa
            viewModel.PokemonsCercanos.CollectionChanged += (_, _) => ActualizarPines();
        }

        // Redibuja todos los pines y centra el mapa en el usuario.
        private void ActualizarPines()
        {
            // En Windows no hay mapa, no hay nada que dibujar
            if (mapa == null) return;

            // Limpia los pines anteriores antes de volver a dibujar
            mapa.Pins.Clear();

            // Oculta el cartelito de selección anterior (nueva búsqueda)
            CartelitoSeleccion.IsVisible = false;

            // Si ya se obtuvo la ubicación, centra el mapa en el usuario (radio de 1 km)
            if (viewModel.UbicacionObtenida)
            {
                var centro = new Location(viewModel.LatitudUsuario, viewModel.LongitudUsuario);
                mapa.MoveToRegion(MapSpan.FromCenterAndRadius(centro, Distance.FromKilometers(1)));
            }

            // Agrega un pin por cada Pokémon cercano
            foreach (var pokemon in viewModel.PokemonsCercanos)
            {
                mapa.Pins.Add(new Pin
                {
                    Label = pokemon.NombreCapitalizado,
                    Address = pokemon.DistanciaTexto,
                    Location = new Location(pokemon.Latitud, pokemon.Longitud),
                    Type = PinType.Place
                });
            }
        }

        // Centra el mapa en el Pokémon tocado y muestra el cartelito con su info.
        private void OnPokemonCercanoTapped(object sender, TappedEventArgs e)
        {
            // El mapa solo existe en Android; en Windows no hay nada que centrar
            if (mapa == null) return;

            // El Pokémon tocado llega en el CommandParameter del gesto (e.Parameter)
            if (e.Parameter is PokemonEnMapa pokemon)
            {
                // Centra y hace zoom cerca de ese Pokémon (radio de 150 m)
                var ubicacion = new Location(pokemon.Latitud, pokemon.Longitud);
                mapa.MoveToRegion(MapSpan.FromCenterAndRadius(ubicacion, Distance.FromMeters(150)));

                // Muestra el cartelito propio con el nombre y la distancia
                CartelitoNombre.Text = pokemon.NombreCapitalizado;
                CartelitoDistancia.Text = pokemon.DistanciaTexto;
                CartelitoSeleccion.IsVisible = true;
            }
        }
    }
}
