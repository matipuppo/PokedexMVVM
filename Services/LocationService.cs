using Microsoft.Maui.Devices.Sensors;

namespace PokeDexMVVM.Services
{
    public class LocationService
    {
        public async Task<Location> ObtenerUbicacionActualAsync()
        {
            var estado = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (estado != PermissionStatus.Granted)
                throw new Exception("Permiso de ubicacion denegado. Otrogar permiso para ver Pokemon cercanos.");

            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var ubicacion = await Geolocation.Default.GetLocationAsync(request);

            if (ubicacion == null)
                throw new Exception("No se pudo obtener la ubicacion. Verifica que el GPS este activado.");

            return ubicacion;
        }
    }
}
