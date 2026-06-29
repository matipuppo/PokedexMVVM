using PokeDexMVVM.Models;
using PokeDexMVVM.Repositories;

namespace PokeDexMVVM.Tests.Integration
{
    // Cada test usa su propia base de datos temporal que se elimina al finalizar
    public class PokemonLocalRepositoryIntegrationTests : IDisposable
    {
        // Ruta de la base de datos temporal
        private readonly string _rutaDb;

        // Instacia del repositorio real que se va a probar
        private readonly PokemonLocalRepository _repositorio;

        // Constuctor: xunit se ejecuta antes de cada test
        // Antes de cada test: se crea base temporal unica y el repositorio real sobre ella.
        public PokemonLocalRepositoryIntegrationTests()
        {
            _rutaDb = Path.Combine(Path.GetTempPath(), $"PokemonTest_{Guid.NewGuid()}.db");
            _repositorio = new PokemonLocalRepository(_rutaDb);
        }

        // Después de cada test: se elimina la base temporal.
        public void Dispose()
        {
            try
            {
                if (File.Exists(_rutaDb))
                    File.Delete(_rutaDb);
            }
            catch (IOException)
            {
                // La conexión pooleada de SQLite-net puede seguir abierta; no es crítico.
            }
        }

        // Favoritos

        // Verifica que al eliminar un favorito, realmente se quita de la base.
        [Fact]
        public async Task EliminarFavorito_SeQuitaDeLaBd()
        {
            // Arrange: se inserta un favorito y se recupera para obtener su Id autogenerado por SQLite
            await _repositorio.AgregarFavoritoAsync(new PokemonFavorito { Nombre = "pikachu", Url = "url1", Imagen = "img1" });
            var favorito = (await _repositorio.ObtenerFavoritosAsync())[0];

            // Act: se elimina por Id
            await _repositorio.EliminarFavoritoAsync(favorito.Id);

            // Assert: la lista queda vacía y EsFavorito devuelve false
            Assert.Empty(await _repositorio.ObtenerFavoritosAsync());
            Assert.False(await _repositorio.EsFavoritoAsync("pikachu"));
        }

        // Verifica que al agregar un favorito, realmente se guarda en la base.
        [Fact]
        public async Task AgregarFavorito_SeGuardaEnLaBd()
        {
            // Act: se inserta un favorito en la base real
            await _repositorio.AgregarFavoritoAsync(new PokemonFavorito { Nombre = "pikachu", Url = "url1", Imagen = "img1" });

            // Assert: EsFavorito lo detecta (se chequea primero, igual que el test de equipo)
            Assert.True(await _repositorio.EsFavoritoAsync("pikachu"));

            // Assert: y además está en la lista con el nombre correcto
            var favoritos = await _repositorio.ObtenerFavoritosAsync();
            Assert.Single(favoritos);
            Assert.Equal("pikachu", favoritos[0].Nombre);
        }

        // Equipo

        // Verifica que al agregar un miembro al equipo, realmente se guarda en la base y el contador devuelve 1.
        [Fact]
        public async Task AgregarAEquipo_SeGuardaYContarDevuelveUno()
        {
            // Act: se inserta un miembro al equipo
            await _repositorio.AgregarAEquipoAsync(new PokemonEquipo { Nombre = "charizard", Url = "url1", Imagen = "img1" });

            // Assert: el contador da 1 y EsEnEquipo lo encuentra
            Assert.Equal(1, await _repositorio.ContarEquipoAsync());
            Assert.True(await _repositorio.EsEnEquipoAsync("charizard"));
        }
        // Verifica que al eliminar un pokemon del equipo por Id, el contador vuelve a 0 y EsEnEquipo ya no lo encuentra.
        [Fact]
        public async Task EliminarDelEquipo_SeQuitaDeLaBd()
        {
            // Arrange: se inserta y se recupera para tener el Id
            await _repositorio.AgregarAEquipoAsync(new PokemonEquipo { Nombre = "charizard", Url = "url1", Imagen = "img1" });
            var miembro = (await _repositorio.ObtenerEquipoAsync())[0];

            // Act: se elimina por Id
            await _repositorio.EliminarEquipoAsync(miembro.Id);

            // Assert: equipo vacío (contador 0) y EsEnEquipo en false
            Assert.Equal(0, await _repositorio.ContarEquipoAsync());
            Assert.False(await _repositorio.EsEnEquipoAsync("charizard"));
        }

        // Verifica que ContarEquipo refleja correctamente la cantidad real de pokemons guardados cuando se agregan varios.
        [Fact]
        public async Task ContarEquipo_ConVariosPokemons_DevuelveCantidadCorrecta()
        {
            // Act: se agregan 3 pokemons distintos al equipo
            await _repositorio.AgregarAEquipoAsync(new PokemonEquipo { Nombre = "charizard", Url = "u1", Imagen = "i1" });
            await _repositorio.AgregarAEquipoAsync(new PokemonEquipo { Nombre = "mewtwo", Url = "u2", Imagen = "i2" });
            await _repositorio.AgregarAEquipoAsync(new PokemonEquipo { Nombre = "snorlax", Url = "u3", Imagen = "i3" });

            // Assert: el contador devuelve 3
            Assert.Equal(3, await _repositorio.ContarEquipoAsync());
        }
    }
}
