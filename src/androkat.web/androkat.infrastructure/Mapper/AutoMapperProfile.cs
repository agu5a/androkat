using androkat.domain.Model;
using androkat.domain.Model.AdminPage;
using androkat.domain.Model.WebResponse;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using System;
using System.Globalization;

namespace androkat.infrastructure.Mapper;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		CreateMap<FixContent, ContentDetailsModel>()
			.ForMember(x => x.FileUrl, y => y.MapFrom(z => string.Empty))
            .ForMember(x => x.Forras, y => y.MapFrom(z => string.Empty))
            .ForMember(x => x.KulsoLink, y => y.MapFrom(z => string.Empty))
            .ForMember(x => x.Img, y => y.MapFrom(z => string.Empty));

		CreateMap<Content, ContentDetailsModel>()
            .ForMember(x => x.Fulldatum, y => y.MapFrom(z => DateTime.Parse(z.Fulldatum, CultureInfo.CreateSpecificCulture("hu-HU"))));

		CreateMap<ContentDetailsModel, Content>()
			.ForMember(x => x.Fulldatum, y => y.MapFrom(z => z.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss")));

		CreateMap<ImaContent, ImaModel>().ReverseMap();

        CreateMap<Maiszent, MaiSzentModel>().ReverseMap();

CreateMap<TempContent, ContentDetailsModel>()
            .ForMember(x => x.Fulldatum, y => y.MapFrom(z => DateTime.Parse(z.Fulldatum, CultureInfo.CreateSpecificCulture("hu-HU"))));
		CreateMap<ContentDetailsModel, TempContent>()
			.ForMember(x => x.Fulldatum, y => y.MapFrom(z => z.Fulldatum.ToString("yyyy-MM-dd HH:mm:s")));

CreateMap<RadioMusor, RadioMusorModel>().ReverseMap();
CreateMap<SystemInfo, SystemInfoModel>().ReverseMap();
		CreateMap<VideoContent, VideoModel>().ReverseMap();
		CreateMap<VideoSource, VideoSourceModel>().ReverseMap();

		CreateMap<Maiszent, ContentDetailsModel>()
            .ForMember(x => x.FileUrl, y => y.MapFrom(z => string.Empty))
            .ForMember(x => x.Forras, y => y.MapFrom(z => string.Empty))
            .ForMember(x => x.KulsoLink, y => y.MapFrom(z => string.Empty));

        CreateMap<ContentResponse, EgyebOlvasnivaloResponse>()
            .ForMember(x => x.KulsoLink, y => y.MapFrom(z => z.KulsoLink))
            .ForMember(x => x.Time, y => y.MapFrom(z => z.Datum))
            .ForMember(x => x.Leiras, y => y.MapFrom(z => z.Idezet));
    }
}