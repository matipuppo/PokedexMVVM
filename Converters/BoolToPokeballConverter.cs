using System.Globalization;

namespace PokeDexMVVM.Converters
{
    // Devuelve icono de pokebola llena si el pokemon esta en el equipo, vacia si no lo esta
    public class BoolToPokeballConverter : IValueConverter
    {
        public object? Convert(object? value, Type targerType, object? parameter, CultureInfo culture)
            => (value is bool b && b) ? "\uE837" : "\uE836";

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
