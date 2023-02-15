using androkat.hu.Models;

namespace androkat.hu.Services;

public interface ISourceData
{
    SourceData GetSourcesFromMemory(int index);
}