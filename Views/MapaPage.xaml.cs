using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using PokeDexMVVM.ViewModels;
using Map = Microsoft.Maui.Controls.Maps.Map;


namespace PokeDexMVVM.Views
{
    public partial class MapaPage : ContentPage
    {
        private readonly MapaViewModel viewModel;
        private Map mapa;

        public MapaPage(MapaViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = viewModel;

            // El control map solo funciona en Android
            if(DeviceInfo.Platform == DevicePlatform.Android)
            {
                mapa = new Map { IsShowingUser = true };
                MapContainer.Content = mapa;
            }
            else
            {
                // En Windows no mostramos nada en el contenedor del mapa
                MapContainer.HeightRequest = 0;
                MapContainer.IsVisible = false;
            }

            viewModel.PokemonsCercanos.CollectionChanged += (_, _) => ActualizarPines();
        }

        private void ActualizarPines()
        {
            if (mapa == null) return;
            mapa.Pins.Clear();
            
            if (viewModel.UbicacionObtenida)
            {
                var centro = new Location(viewModel.LatitudUsuario, viewModel.LongitudUsuario);
                mapa.MoveToRegion(MapSpan.FromCenterAndRadius(centro, Distance.FromKilometers(1)));
            }

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
    }
}
