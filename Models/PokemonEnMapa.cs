namespace PokeDexMVVM.Models
{
    public class PokemonEnMapa
    {
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public double DistanciaMetros { get; set; }

        public string NombreCapitalizado =>
            string.IsNullOrEmpty(Nombre) ? Nombre : char.ToUpper(Nombre[0]) + Nombre.Substring(1);

        public string DistanciaTexto =>
            DistanciaMetros < 1000 ? $"{DistanciaMetros:0} m" : $"{DistanciaMetros / 1000:0.0} km";
    }
}
