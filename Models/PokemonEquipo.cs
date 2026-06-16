using SQLite;

namespace PokeDexMVVM.Models
{
    // Clase que representa un equipo de Pokémon en la base de datos SQLite
    [Table("Equipo")]
    public class PokemonEquipo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Url { get; set; }
        public string Imagen { get; set; }

        public string NombreCapitalizado
        {
            get
            {
                if (string.IsNullOrEmpty(Nombre)) return Nombre;
                return char.ToUpper(Nombre[0]) + Nombre.Substring(1);
            }
        }
    }
}
