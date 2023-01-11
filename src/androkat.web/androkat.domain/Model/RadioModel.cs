namespace androkat.domain.Model;

public class RadioModel
{
	public RadioModel(string name, string url)
	{
		Name = name;
		Url = url;
	}

	public string Name { get; }
    public string Url { get; }
}
