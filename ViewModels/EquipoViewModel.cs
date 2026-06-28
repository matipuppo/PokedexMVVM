using PokeDexMVVM.Models;
using PokeDexMVVM.Repositories;
using PokeDexMVVM.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PokeDexMVVM.ViewModels
{
    // View Model que maneja la logica de la pantalla de equipo
    public class EquipoViewModel : BaseViewModel
    {
        // Repositorio local para acceder a la bd
        private readonly IPokemonLocalRepository repositorio;

        // Lista de pokemons del equipo que se muestra en la pantalla
        public ObservableCollection<PokemonEquipo> Equipo { get; set; } = new();

        // Mensaje de estado para mostrar en la UI
        private string mensajeEstado;
        public string MensajeEstado
        {
            get => mensajeEstado;
            set => SetProperty(ref mensajeEstado, value);
        }

        // Comando para eliminar un pokemon del equipo
        public ICommand EliminarDelEquipoCommand { get; }

        //Constructor que recibe el repositorio por DI
        public EquipoViewModel(IPokemonLocalRepository repositorio)
        {
            this.repositorio = repositorio;
            EliminarDelEquipoCommand = new Command<PokemonEquipo>(async (pokemon) => await EliminarDelEquipoAsync(pokemon));
        }

        // Cargar equipo desde la bd
        public async Task CargarEquipoAsync()
        {
            try
            {
                MensajeEstado = "Cargando Equipo...";
                var lista = await repositorio.ObtenerEquipoAsync();
                Equipo.Clear();
                foreach (var pokemon in lista)
                    Equipo.Add(pokemon);

                MensajeEstado = Equipo.Count == 0 ? "Tu equipo esta vacio." : string.Empty;
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al cargar equipo: {ex.Message}";
            }
        }
        
        // Elimina un pokemon del equipo y recarga la lista
        public async Task EliminarDelEquipoAsync(PokemonEquipo pokemon)
        {
            await repositorio.EliminarEquipoAsync(pokemon.Id);
            await CargarEquipoAsync();
        }
    }
}
