using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using System;

namespace androkat.infrastructure.Mapper;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		CreateMap<FixContent, ContentDetailsModel>()
            .ForMember(x => x.Fulldatum, y => y.MapFrom(z => Get(z.Fulldatum)))
			.ForMember(x => x.FileUrl, y => y.MapFrom(z => string.Empty))
            .ForMember(x => x.Inserted, y => y.MapFrom(z => DateTime.UtcNow))
            .ForMember(x => x.Forras, y => y.MapFrom(z => string.Empty))
            .ForMember(x => x.KulsoLink, y => y.MapFrom(z => string.Empty))
            .ForMember(x => x.Img, y => y.MapFrom(z => string.Empty));

		CreateMap<Content, ContentDetailsModel>()
			.ForMember(x => x.Fulldatum, y => y.MapFrom(z => DateTime.Parse(z.Fulldatum)));

		CreateMap<ContentDetailsModel, Content>()
			.ForMember(x => x.Fulldatum, y => y.MapFrom(z => z.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss")));

		CreateMap<ImaContent, ImaModel>().ReverseMap();

CreateMap<TempContent, ContentDetailsModel>()
			.ForMember(x => x.Fulldatum, y => y.MapFrom(z => DateTime.Parse(z.Fulldatum)));
		CreateMap<ContentDetailsModel, TempContent>()
			.ForMember(x => x.Fulldatum, y => y.MapFrom(z => z.Fulldatum.ToString("yyyy-MM-dd HH:mm:s")));

CreateMap<RadioMusor, RadioMusorModel>().ReverseMap();
CreateMap<SystemInfo, SystemInfoModel>().ReverseMap();
		CreateMap<VideoContent, VideoModel>().ReverseMap();
		CreateMap<VideoSource, VideoSourceModel>().ReverseMap();

		CreateMap<Maiszent, ContentDetailsModel>()
            .ForMember(x => x.Fulldatum, y => y.MapFrom(z => DateTime.Parse(DateTime.Now.ToString("yyyy-") + z.Fulldatum + " 00:00:01")))            
            .ForMember(x => x.FileUrl, y => y.MapFrom(z => string.Empty))
            .ForMember(x => x.Forras, y => y.MapFrom(z => string.Empty))
            .ForMember(x => x.KulsoLink, y => y.MapFrom(z => string.Empty));
	}

private static DateTime Get(string honapNap)
    {
        if (honapNap == "02-29")
            return DateTime.Parse("2024-" + honapNap + " 00:00:01");
        else
            return DateTime.Parse(DateTime.UtcNow.ToString("yyyy-") + honapNap + " 00:00:01");

    }
}