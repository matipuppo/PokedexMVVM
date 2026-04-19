using System.Collections.Generic;           // Para usar List<T>
using System.Text.Json.Serialization;       // Para usar JsonPropertyName y mapear las propiedades a los nombres de la API

namespace PokeDexMVVM.Models
{
    // Clase que respresenta la rta de la API cuando pedimos la lista de pokemon
    internal class ListaPokemon
    {
        // La propiedad "results" de la respuesta de la API se mapea a esta lista de resultados
        // Contiene objetos de tipo PokemonResult, que tienen el nombre y la URL de cada pokemon
        [JsonPropertyName("results")]
        public List<PokemonResult> Resultados { get; set; }
    }
}
