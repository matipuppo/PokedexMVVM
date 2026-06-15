using SQLite;

namespace PokeDexMVVM.Models
{
    // Clase que representa un Pokémon favorito en la base de datos SQLite

    [Table("Favoritos")]
    public class PokemonFavorito
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
                if(string.IsNullOrEmpty(Nombre)) return Nombre;
                return char.ToUpper(Nombre[0]) + Nombre.Substring(1);
            }
         
        }
    }
}
