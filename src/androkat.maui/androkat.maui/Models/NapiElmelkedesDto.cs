using SQLite;

namespace androkat.hu.Models;

[Table("napielmelkedesv1")]
public class NapiElmelkedesDto
{
    [PrimaryKey]
    public Guid nid { get; set; }

    /// <summary>
    /// recorddate, time
    /// </summary>
    public DateTime datum { get; set; }

    /// <summary>
    /// title
    /// </summary>
    //[MaxLength(250), Unique]
    public string cim { get; set; }

    /// <summary>
    /// content, leiras
    /// </summary>
    public string idezet { get; set; }
    public string tipus { get; set; }
    public string img { get; set; }
    public string forras { get; set; }
    public string TypeName { get; set; }
    public bool IsRead { get; set; }
    public string KulsoLink { get; set; }
    public string GroupName { get; set; }
}