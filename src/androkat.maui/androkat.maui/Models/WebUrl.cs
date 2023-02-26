namespace androkat.hu.Models;

public class WebUrl
{
    public WebUrl(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public string Name { get; }
    public string Url { get; }
}