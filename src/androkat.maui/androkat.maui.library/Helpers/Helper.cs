using System.Text.Json.Serialization;
using System.Text.Json;

namespace androkat.maui.library.Helpers;

internal static class Helper
{
    public static string ReplaceUtf8(string text)
    {
        return text.Replace("&#x0151;", "ő").Replace("&#x0150;", "Ő").Replace("&#x0171;", "ű").Replace("&#x0170;", "Ű")
                .Replace("&#x0246;", "ö").Replace("&#x0214;", "Ö").Replace("&#x0252;", "ü").Replace("&#x0220;", "Ü")
                .Replace("&ouml;", "ö").Replace("&Ouml;", "Ö").Replace("&uuml;", "ü").Replace("&Uuml;", "Ü");
    }

    /*public Intent shareIntent(string title, string body, IHtml html)
    {
        Intent shareIntent = new Intent(Intent.ACTION_SEND);
        shareIntent.setType("text/html");
        string separator = "";
        if (!TextUtils.isEmpty(title)) separator = ": ";
        shareIntent.putExtra(Intent.EXTRA_SUBJECT, "AndroKat" + separator + title);
        shareIntent.putExtra(Intent.EXTRA_TEXT, fromHtml(body, html));
        return shareIntent;
    }*/
}

public class DateTimeConverterUsingDateTimeParse : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString() ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
