using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace androkat.infrastructure.Model.SQLite;

[Table("admin")]
public class Admin
{
    [Key]
    [Required]
    [DataMember(Name = "id")]
    public int Id { get; set; }

    [DataMember(Name = "username")]
    public string UserName { get; set; }

    [DataMember(Name = "pwd")]
    public string Pwd { get; set; }

    [DataMember(Name = "email")]
    public string Email { get; set; }

    [DataMember(Name = "lastlogin")]
    public DateTime LastLogin { get; set; } //"yyyy-MM-dd HH:mm:ss"
}