using System.Globalization;

namespace PokeDexMVVM.Converters
{
    public class StatToProgressConverter : IValueConverter
    {
        public object Convert(object? value, Type targuetType, object? parameter, CultureInfo culture)
        {
            if (value is int stat)
                return Math.Clamp(stat / 255.0, 0.0, 1.0);
            return 0.0;
        }

        public object ConvertBack(object? value, Type targuetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
