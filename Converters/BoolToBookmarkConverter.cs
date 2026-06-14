using System.Globalization;

namespace PokeDexMVVM.Converters
{
    // Devuelve el ícono de marcador relleno si es favorito, o vacío si no lo es
    public class BoolToBookmarkConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool esFavorito)
                return esFavorito ? "\uE866" : "\uE867";
            return "\uE867";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
