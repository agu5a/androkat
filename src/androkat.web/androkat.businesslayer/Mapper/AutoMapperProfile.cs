using androkat.businesslayer.Model;
using androkat.datalayer.Model;
using androkat.datalayer.Model.SQLite;
using AutoMapper;
using System;

namespace androkat.businesslayer.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<IdezetData, IdezetDataViewModel>();
        CreateMap<Napiolvaso, NapiOlvasoViewModel>()
            .ForMember(x => x.Datum, y => y.MapFrom(z => DateTime.Parse(z.Datum)));
    }
}