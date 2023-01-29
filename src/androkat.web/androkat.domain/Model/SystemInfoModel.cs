namespace androkat.domain.Model;

public class SystemInfoModel
{
    public SystemInfoModel(int id, string key, string value)
    {
        Id = id;
        Key = key;
        Value = value;
    }

    public int Id { get; }
    public string Key { get; }
    public string Value { get; }
}