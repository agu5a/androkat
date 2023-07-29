namespace androkat.domain.Model.AdminPage;

public class BaseData
{
    public string Cim { get; set; }
    public string Tipus { get; set; }
}

public class ImgData : BaseData
{
    public string Img { get; set; }
}

public class FileData : BaseData
{    
    public string Path { get; set; }    
}