namespace androkat.hu.Helpers;

public class FileAccessHelper
{
    public static string GetLocalFilePath(string filename)
    {
        return Path.Combine(FileSystem.AppDataDirectory, filename);
    }
}