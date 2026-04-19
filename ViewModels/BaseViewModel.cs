using System.ComponentModel;   // Me permite usar INotifyPropertyChanged
using System.Runtime.CompilerServices;  // Me permite usar CallerMemberName

namespace PokeDexMVVM.ViewModels
{
    // Clase base para los ViewModels, implementa INotifyPropertyChanged para notificar cambios a la UI
    public class BaseViewModel : INotifyPropertyChanged
    {
        // cuando una propiedad del ViewModel cambia, se dispara este evento para que la UI se actualice
        public event PropertyChangedEventHandler PropertyChanged;

        //Metodo que notifica a la UI cuando una propiedad cambia
        // CallerMemberName permite que se use el nombre de la popiedad automáticamente si no se especifica
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Metodo auxiliar para asignar valores y notificar cambios
        protected bool SetProperty<T>(ref T campo, T valor, [CallerMemberName] string propiedad = null)
        {
            // si el valor es igual al campo actual, no se hace nada
            if (Equals(campo, valor))
                return false;

            // asignamos un nuevo valor
            campo = valor;

            // Notificamos a la UI que la propiedad cambio
            OnPropertyChanged(propiedad);
            return true;
        }
    }
}
