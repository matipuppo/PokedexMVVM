using System.Collections.Generic;           // se utiliza para List<T>
using System.Text.Json.Serialization;       // se usa para mapear las propiedades JSON a C#

namespace PokeDexMVVM.Models
{
    // Clase que representa el detalle de un Pokémon obtenido de la API
    public class DetallePokemon
    {
        // Mapeamos la propiedad "weight" del JSON a la propiedad "Peso" en C#
        [JsonPropertyName("weight")]
        public int Peso { get; set; }

        // Mapeamos la propiedad "height" del JSON a la propiedad "Altura" en C#
        [JsonPropertyName("height")]
        public int Altura { get; set; }

        // Mapeamos la propiedad "types" del JSON a la lista "Tipos" en C#
        [JsonPropertyName("types")]
        public List<TipoPokemon> Tipos { get; set; }

        // Mapeamos la propiedad "sprites" del JSON a la propiedad "Sprites" en C#
        [JsonPropertyName("sprites")]
        public Sprites Sprites { get; set; }

        // Mapeamos la propiedad "stats" del JSON a la lista "Stats" en C# HP - Ataque - Defensa
        [JsonPropertyName("stats")]
        public List<StatWrapper> Stats { get; set; }
    }

    // Claseintermedia que representa el tipo de Pokémon dentro de la lista de tipos del JSON
    public class TipoPokemon
    {
        [JsonPropertyName("type")]
        public Tipo Tipo { get; set; }
    }

    // Clase que representa un tipo de Pokémon con su nombre en inglés y una propiedad calculada para su traducción al español
    public class Tipo
    {
        [JsonPropertyName("name")]
        public string Nombre { get; set; }

        // Propiedad calculada que traduce el nombre del tipo al español
        public string NombreEspañol => Nombre switch
        {
            "grass" => "Planta",
            "poison" => "Veneno",
            "fire" => "Fuego",
            "water" => "Agua",
            "electric" => "Eléctrico",
            "bug" => "Bicho",
            "normal" => "Normal",
            "flying" => "Volador",
            "psychic" => "Psíquico",
            "rock" => "Roca",
            "ground" => "Tierra",
            "ice" => "Hielo",
            "dragon" => "Dragón",
            "dark" => "Siniestro",
            "steel" => "Acero",
            "fairy" => "Hada",
            _ => Nombre // Si no se encuentra una traducción, se devuelve el nombre original
        };
    }

    // Clase que representa las imágenes (sprites) de un Pokémon obtenidas del JSON (imagenes)
    public class Sprites
    {
        [JsonPropertyName("front_default")]
        public string FrontDefault { get; set; }
    }

    // Clase que representa una estadística de un Pokémon, con su valor base y el nombre de la estadística
    public class StatWrapper
    {
        [JsonPropertyName("base_stat")]
        public int Valor { get; set; }

        [JsonPropertyName("stat")]
        public StatInfo Stat { get; set; }
    }

    // Clase que representa la información de una estadística, con su nombre (HP, Ataque, Defensa, etc.)
    public class StatInfo
    {
        [JsonPropertyName("name")]
        public string Nombre { get; set; }
    }
}

