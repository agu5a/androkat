using androkat.datalayer.Model.SQLite;
using androkat.domain.Model;
using AutoMapper;
using System;

namespace androkat.datalayer.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<IdezetData, IdezetDataViewModel>();

        CreateMap<Napiolvaso, NapiOlvasoViewModel>()
            .ForMember(x => x.Fulldatum, y => y.MapFrom(z => DateTime.Parse(z.Fulldatum)));
    }
}