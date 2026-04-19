using System.Text.Json.Serialization;       // Para usar JsonPropertyName y mapear las propiedades JSON a las propiedades de la clase

namespace PokeDexMVVM.Models
{
    // Clase que representa un resultado de Pokémon en la lista obtenida de la API
    public class PokemonResult
    {
        // Mapear la propiedad "name" del JSON a la propiedad "Nombre" de la clase
        [JsonPropertyName("name")]
        public string Nombre { get; set; }

        // Mapear la propiedad "url" del JSON a la propiedad "Url" de la clase
        [JsonPropertyName("url")]
        public string Url { get; set; }

        // Propiedad adicional para almacenar la URL de la imagen del Pokémon
        public string Imagen { get; set; }

        // Propiedad calculada para mostrar con mayúscula inicial
        public string NombreCapitalizado
        {
            get
            {
                if (string.IsNullOrEmpty(Nombre))
                    return Nombre;

                return char.ToUpper(Nombre[0]) + Nombre.Substring(1);
            }
        }
    }
}
