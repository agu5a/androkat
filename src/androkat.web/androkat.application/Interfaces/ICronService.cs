using System.Collections.Generic;

namespace androkat.application.Interfaces;

public interface ICronService
{
    void DeleteCaches();
    List<string> DeleteFiles(string webRootPath, bool shouldDelete = false);
    int Start();
}