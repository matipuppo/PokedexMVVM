using System.Globalization;

namespace PokeDexMVVM.Converters
{
    // Devuelve el nombre del archivo de imagen según si el pokemon está en el equipo o no
    public class BoolToPokeballImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => (value is bool b && b) ? "pokeball_color.png" : "pokeball_outline.png";

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
