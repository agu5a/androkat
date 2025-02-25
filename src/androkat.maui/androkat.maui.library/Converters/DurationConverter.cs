﻿#nullable enable
using System.Globalization;

namespace androkat.maui.library.Converters;

public class DurationConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return value;
        }

        var result = string.Empty;

        if (value is string stringValue)
        {
            try
            {
                var duration = TimeSpan.Parse(stringValue, CultureInfo.CurrentCulture);
                result = $"{duration.TotalMinutes:N0} min";
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return result;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
