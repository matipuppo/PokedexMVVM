using PokeDexMVVM.Models;
using PokeDexMVVM.Tests.Fakes;
using PokeDexMVVM.ViewModels;


namespace PokeDexMVVM.Tests;

// Text del favoritosviewmodel usando repositorio falso en memoria
public class favoritosViewModelTests
{
    // Verifica que al cargar favoritos con la lista vacia, se muestre el mensaje correcto
    [Fact]
    public async Task CargarFavoritos_ListaVacia_MuestraMensaje()
    {
        var repository = new PokemonLocalRepositoryFake();
        var viewModel = new FavoritosViewModel(repository);

        await viewModel.CargarFavoritosAsync();

        Assert.Empty(viewModel.Favoritos);
        Assert.Equal("No tenes Pokemons favoritos guardados.", viewModel.MensajEstado);
    }

    //Verifica que al cargar favoritos con items, la lista se llegue correctamente

    [Fact]
    public async Task CargarFavoritos_ConItems_ListaLlena()
    {
        var repository = new PokemonLocalRepositoryFake();

        await repository.AgregarFavoritoAsync(new PokemonFavorito { Nombre = "pikachu", Url = "url1", Imagen = "img1" });
        await repository.AgregarFavoritoAsync(new PokemonFavorito { Nombre = "bulbasor", Url = "url2", Imagen = "img2" });

        var viewModel = new FavoritosViewModel(repository);
        await viewModel.CargarFavoritosAsync();

        Assert.Equal(2, viewModel.Favoritos.Count);
        Assert.Equal(string.Empty, viewModel.MensajEstado);

    }

    // Verifica que al eliminar un favorito, desaparezca de la lista y del repositorio
    [Fact]
    public async Task EliminarFavorito_EliminarDeListayBD()
    {
        var repository = new PokemonLocalRepositoryFake();
        await repository.AgregarFavoritoAsync(new PokemonFavorito { Nombre = "pikachu", Url = "url1", Imagen = "img1" });

        var viewModel = new FavoritosViewModel(repository);
        await viewModel.CargarFavoritosAsync();

        var pokemon = viewModel.Favoritos[0];
        await viewModel.EliminarFavoritosAsync(pokemon);

        Assert.Empty(viewModel.Favoritos);
        Assert.False(await repository.EsFavoritoAsync("pikachu"));
    }
}
    // Tests del ViewModel del Equipo usando el repositorio falso en memoria
 public class EquipoViewModelTests
 {
    // Verifica que al cargar el equipo vacío, se muestre el mensaje correcto
    [Fact]
     public async Task CargarEquipo_ListaVacia_MuestraMensaje()
     {
         var repository = new PokemonLocalRepositoryFake();
         var viewModel = new EquipoViewModel(repository);
         await viewModel.CargarEquipoAsync();

         Assert.Empty(viewModel.Equipo);
         Assert.Equal("Tu equipo esta vacio.", viewModel.MensajeEstado);
      }

     // Verifica que al cargar el equipo con items, la lista se llene correctamente
     [Fact]
     public async Task CargarEquipo_ConItems_LlenaLista()
     {
         var repository = new PokemonLocalRepositoryFake();
         await repository.AgregarAEquipoAsync(new PokemonEquipo { Nombre = "charizard", Url = "url1", Imagen = "img1" });
         await repository.AgregarAEquipoAsync(new PokemonEquipo { Nombre = "mewtwo", Url = "url2", Imagen = "img2" });

         var viewModel = new EquipoViewModel(repository);
         await viewModel.CargarEquipoAsync();

         Assert.Equal(2, viewModel.Equipo.Count);
         Assert.Equal(string.Empty, viewModel.MensajeEstado);
        }

      // Verifica que al eliminar un pokemon del equipo, desaparezca de la lista y del repositorio
      [Fact]
      public async Task EliminarDelEquipo_EliminaDeListaYBd()
      {
          var repository = new PokemonLocalRepositoryFake();
          await repository.AgregarAEquipoAsync(new PokemonEquipo { Nombre = "charizard", Url = "url1", Imagen = "img1" });

          var viewModel = new EquipoViewModel(repository);
          await viewModel.CargarEquipoAsync();

          var pokemon = viewModel.Equipo[0];
          await viewModel.EliminarDelEquipoAsync(pokemon);

          Assert.Empty(viewModel.Equipo);
          Assert.False(await repository.EsEnEquipoAsync("charizard"));
      }
  }