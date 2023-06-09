using androkat.maui.library.Models.Responses;

namespace androkat.maui.library.Models;

public class Category
{
    public Category(CategoryResponse response)
    {
        Id = response.Id;
        Genre = response.Genre;
    }

    public Guid Id { get; set; }

    public string Genre { get; set; }
}
