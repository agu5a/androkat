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
			.ForMember(x => x.Fulldatum, y => y.MapFrom(z => DateTime.Parse(DateTime.Now.ToString("yyyy-") + z.Datum + " 00:00:01")))
			.ForMember(x => x.FileUrl, y => y.MapFrom(z => string.Empty));

		CreateMap<Content, ContentDetailsModel>()
			.ForMember(x => x.Fulldatum, y => y.MapFrom(z => DateTime.Parse(z.Fulldatum)));

		CreateMap<ContentDetailsModel, Content>()
			.ForMember(x => x.Fulldatum, y => y.MapFrom(z => z.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss")));

		CreateMap<ImaContent, ImaModel>().ReverseMap();

CreateMap<RadioMusor, RadioMusorModel>().ReverseMap();
CreateMap<SystemInfo, SystemInfoModel>().ReverseMap();
		CreateMap<VideoContent, VideoModel>().ReverseMap();
		CreateMap<VideoSource, VideoSourceModel>().ReverseMap();

		CreateMap<Maiszent, ContentDetailsModel>()
			.ForMember(x => x.Fulldatum, y => y.MapFrom(z => DateTime.Parse(DateTime.Now.ToString("yyyy-") + z.Datum + " 00:00:01")))
			.ForMember(x => x.Tipus, y => y.MapFrom(z => (int)Forras.maiszent))
			.ForMember(x => x.FileUrl, y => y.MapFrom(z => string.Empty));
	}
}