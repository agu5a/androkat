﻿using SQLite;
using System.Runtime.Serialization;

namespace androkat.maui.library.Models.Entities;

[Table("bunokv1")]
public class Bunok
{
    [PrimaryKey]
    [DataMember(Name = "nid")]
    public Guid Nid { get; set; }

    [DataMember(Name = "bunId")]
    public int BunId { get; set; }

    [DataMember(Name = "parancsId")]
    public int ParancsId { get; set; }
}
