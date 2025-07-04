namespace androkat.maui.library.Models;

public class PrayerCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public override string ToString() => Name;
}
