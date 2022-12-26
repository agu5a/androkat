using androkat.domain.Model;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using System;

namespace androkat.infrastructure.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Napiolvaso, ContentDetailsModel>()
            .ForMember(x => x.Fulldatum, y => y.MapFrom(z => DateTime.Parse(z.Fulldatum)));

        CreateMap<ContentDetailsModel, Napiolvaso>()
            .ForMember(x => x.Fulldatum, y => y.MapFrom(z => z.Fulldatum.ToString("yyyy-MM-dd HH:mm:s")));        
    }
}