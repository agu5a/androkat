namespace androkat.maui.library.Abstraction;

public interface IResourceData
{
    Task<string> GetResourceAsString(string filename);
}