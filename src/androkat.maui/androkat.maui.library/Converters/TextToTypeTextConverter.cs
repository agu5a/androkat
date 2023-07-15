using System.Globalization;
using System.Text.RegularExpressions;

namespace androkat.maui.library.Converters;

public partial class TextToTypeTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isHtml = ContainsHTML(value as string);

        return isHtml
            ? TextType.Html
            : TextType.Text;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static bool ContainsHTML(string text)
    {
        return !string.IsNullOrWhiteSpace(text) && HtmlRegex().IsMatch(text);
    }

    [GeneratedRegex("<(.|\n)*?>")]
    private static partial Regex HtmlRegex();
}
