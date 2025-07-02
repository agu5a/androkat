#nullable enable
using System.Globalization;

namespace androkat.maui.library.Converters;

public class BooleanToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue && parameter is string parameterString)
        {
            var texts = parameterString.Split('|');
            if (texts.Length == 2)
            {
                return boolValue ? texts[1] : texts[0];
            }
        }
        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
