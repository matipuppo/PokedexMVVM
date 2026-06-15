using PokeDexMVVM.Models;

namespace PokeDexMVVM.Repositories
{

    public interface IPokemonLocalRepository
    {

        // Favoritos

        // Devuelve una lista de todos los pokemons guardados como favoritos en la bd
        Task<List<PokemonFavorito>> ObtenerFavoritosAsync();

        // Guarda un pokemon como favorito en la bd
        Task AgregarFavoritoAsync(PokemonFavorito pokemon); 

        // Elimina un pokemon de favoritos en la bd x id
        Task EliminarFavoritoAsync(int id);

        // Verifica si un pokemon es favorito en la bd
        Task<bool> EsFavoritoAsync(string nombre);

        // Equipo

        // Devuelve una lista de todos los pokemons guardados en el equipo en la bd
        Task<List<PokemonEquipo>> ObtenerEquipoAsync();

        // Guarda un pokemon en el equipo en la bd
        Task AgregarAEquipoAsync(PokemonEquipo pokemon);

        // Elimina un pokemon del equipo en la bd x id
        Task EliminarEquipoAsync(int id);

        // Devuelve el número de pokemons en el equipo en la bd
        Task<int> ContarEquipoAsync();

        // Verifica si un pokemon ya esa en el equipo en la bd
        Task<bool> EsEnEquipoAsync(string nombre);

    }
}
