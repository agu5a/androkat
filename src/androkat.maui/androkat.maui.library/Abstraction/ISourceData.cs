using androkat.maui.library.Models;

namespace androkat.maui.library.Abstraction;

public interface ISourceData
{
    SourceData GetSourcesFromMemory(int index);
}