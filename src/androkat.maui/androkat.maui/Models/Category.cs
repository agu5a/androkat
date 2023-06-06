using androkat.hu.Models.Responses;

namespace androkat.hu.Models;

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
