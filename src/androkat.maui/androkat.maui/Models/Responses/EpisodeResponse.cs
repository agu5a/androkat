namespace androkat.hu.Models.Responses;

public class EpisodeResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime Published { get; set; }

    public string Duration { get; set; }

    public Uri Url { get; set; }
}
