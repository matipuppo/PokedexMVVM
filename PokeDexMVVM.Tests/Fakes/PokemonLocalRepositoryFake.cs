using PokeDexMVVM.Models;
using PokeDexMVVM.Repositories;

namespace PokeDexMVVM.Tests.Fakes
{
    //Implementacion falsa de repositorios que usa listas en memoria
    public class PokemonLocalRepositoryFake : IPokemonLocalRepository
    {
        //Listas en memoria que simulan las tablas de la bd
        private readonly List<PokemonFavorito> _favoritos = new();
        private readonly List<PokemonEquipo> _equipo = new();

        private int _nextId = 1;


        // Favoritos

        // Devuelve una copia de la lista de favoritos en memoria
        public Task<List<PokemonFavorito>> ObtenerFavoritosAsync() =>
            Task.FromResult(new List<PokemonFavorito>(_favoritos));

        // Agregar un favorito asignandole un ID autoincrement

        public Task AgregarFavoritoAsync(PokemonFavorito pokemon)
        {
            pokemon.Id = _nextId++;
            _favoritos.Add(pokemon);
            return Task.CompletedTask;

        }

        // Elimina el favorito que tenga el Id indicado
        public Task EliminarFavoritoAsync(int id)
        {
            _favoritos.RemoveAll(p => p.Id == id);
            return Task.CompletedTask;
        }

        // Verifica si existe un favorito con ese nombre
        public Task<bool> EsFavoritoAsync(string nombre) =>
            Task.FromResult(_favoritos.Any(p => p.Nombre == nombre));


        // Equipo

        // Devuelve una copia de la lista de equipo en memoria
        public Task<List<PokemonEquipo>> ObtenerEquipoAsync() =>
            Task.FromResult(new List<PokemonEquipo>(_equipo));

        // Agregar un pokemon al equipo asignandole un ID autoincrement

        public Task AgregarAEquipoAsync(PokemonEquipo pokemon)
        {
            pokemon.Id = _nextId++;
            _equipo.Add(pokemon);
            return Task.CompletedTask;

        }

        // Elimina un pokmeon del equipo que tenga el Id indicado
        public Task EliminarEquipoAsync(int id)
        {
            _equipo.RemoveAll(p => p.Id == id);
            return Task.CompletedTask;
        }

        // Verifica si existe un pokemon en el equipo con ese nombre
        public Task<bool> EsEnEquipoAsync(string nombre) =>
            Task.FromResult(_equipo.Any(p => p.Nombre == nombre));

        // Devuelve la cantidad de pokemon en el equipo
        public Task<int> ContarEquipoAsync() =>
            Task.FromResult(_equipo.Count);
    }
}



