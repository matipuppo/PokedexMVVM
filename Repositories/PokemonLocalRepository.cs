using SQLite;
using PokeDexMVVM.Models;

namespace PokeDexMVVM.Repositories
{
    // Guarda y lee los datos de favoritos y de equipo en la bd del dispositivo usando SQLite
    public class PokemonLocalRepository : IPokemonLocalRepository
    {
        // Conexion asincornica a la bd usando SQLite
        private readonly SQLiteAsyncConnection _db;

        // Constructor que inicializa la conexion a la bd y crea las tablas si no existen
        public PokemonLocalRepository(string rutaDb)
        { 
            _db = new SQLiteAsyncConnection(rutaDb);
            _db.CreateTableAsync<PokemonFavorito>().Wait();
            _db.CreateTableAsync<PokemonEquipo>().Wait();
        }

        // Favorito

        // Muestra todos los pokemons guardados como favoritos en la bd
        public Task<List<PokemonFavorito>> ObtenerFavoritosAsync() =>
            _db.Table<PokemonFavorito>().ToListAsync();

        // Agrega un pokemon a la lista de favoritos en la bd
        public Task AgregarFavoritoAsync(PokemonFavorito pokemon) =>
            _db.InsertAsync(pokemon);


        // Elimina un pokemon de la lista de favoritos en la bd usando su id
        public Task EliminarFavoritoAsync(int id) =>
            _db.DeleteAsync<PokemonFavorito>(id);

        // Busca en la tabla si existe algun pokemon con ese nombre y devuelve true si lo encuentra
        public async Task<bool> EsFavoritoAsync(string nombre) =>
            await _db.Table<PokemonFavorito>()
                       .Where(p => p.Nombre == nombre)
                        .CountAsync() > 0;


        // Equipo

        // Muestra todos los pokemons guardados en el equipo en la bd
        public Task<List<PokemonEquipo>> ObtenerEquipoAsync() =>
            _db.Table<PokemonEquipo>().ToListAsync();


        // Agrega un pokemon a la lista del equipo en la bd
        public Task AgregarAEquipoAsync(PokemonEquipo pokemon) =>
            _db.InsertAsync(pokemon);
        
        // Elimina un pokemon de la lista del equipo en la bd usando su id
        public Task EliminarEquipoAsync(int id) =>
            _db.DeleteAsync<PokemonEquipo>(id);

        // Devuelve el numero de pokemons guardados en el equipo en la bd
        public Task<int> ContarEquipoAsync() =>
            _db.Table<PokemonEquipo>().CountAsync();

        // Verifica si un pokemon ya esta en el equipo buscando por nombre
        public async Task<bool> EsEnEquipoAsync(string nombre) =>
            await _db.Table<PokemonEquipo>()
                       .Where(p => p.Nombre == nombre)
                        .CountAsync() > 0;
    }
}
