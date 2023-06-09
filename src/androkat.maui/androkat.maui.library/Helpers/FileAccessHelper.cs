namespace androkat.maui.library.Helpers;

public class FileAccessHelper
{
    public static string GetLocalFilePath(string filename)
    {
        return Path.Combine(FileSystem.AppDataDirectory, filename);
    }
}