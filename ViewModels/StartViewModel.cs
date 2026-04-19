using System.Windows.Input;

namespace PokeDexMVVM.ViewModels
{
    // Hereda de BaseViewModel para tener acceso a la funcionalidad de INotifyPropertyChanged
    public class StartViewModel : BaseViewModel
    {
        //Comando que se ejecutará al hacer clic en el botón "Cargar Pokemon"
        public ICommand IrAPokedexCommand { get; }

        // Constructor del ViewModel
        public StartViewModel()
        {
            // Comando asincrono que navega a la MainPage al ejecutarse
            IrAPokedexCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(Views.MainPage));
            });
        }
    }
}


