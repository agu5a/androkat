using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Model;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.infrastructure.DataManager;

public class ApiRepository : BaseRepository, IApiRepository
{
    public ApiRepository(AndrokatContext ctx,
        IClock clock,
        IMapper mapper) : base(ctx, clock, mapper)
    {
    }

    public bool DeleteContentDetailsByNid(Guid nid)
    {
        var res = Ctx.Content.FirstOrDefault(f => f.Nid == nid);
        if (res is null)
        {
            return false;
        }

        Ctx.Content.Remove(res);
        Ctx.SaveChanges();
        return true;
    }

    public bool UpdateRadioMusor(RadioMusorModel radioMusorModel)
    {
        var old = Ctx.RadioMusor.FirstOrDefault(w => w.Source == radioMusorModel.Source);
        if (old is null)
        {
            return false;
        }

        old.Inserted = radioMusorModel.Inserted;
        old.Musor = radioMusorModel.Musor;
        Ctx.SaveChanges();
        return true;
    }

    public bool UpdateRadioSystemInfo(string value)
    {
        if (Ctx.SystemInfo.FirstOrDefault(w => w.Key == "radio") is null)
        {
            return false;
        }

            Ctx.SystemInfo.First(w => w.Key == "radio").Value = value;
            Ctx.SaveChanges();
            return true;
        }

    public IEnumerable<SystemInfoModel> GetSystemInfoModels()
    {
        var res = Ctx.SystemInfo.AsQueryable();
        return Mapper.Map<IEnumerable<SystemInfoModel>>(res);
    }

    public bool AddContentDetailsModel(ContentDetailsModel contentDetailsModel)
    {
        var exist = Ctx.Content.FirstOrDefault(w => w.Tipus == contentDetailsModel.Tipus
        && (w.Cim.Contains(contentDetailsModel.Cim) || w.Nid == contentDetailsModel.Nid));
        if (exist is not null)
        {
            return false;
        }

        Ctx.Content.Add(Mapper.Map<Content>(contentDetailsModel));
        Ctx.SaveChanges();
        return true;
    }

    public IEnumerable<ContentDetailsModel> GetContentDetailsModels()
    {
        var res = Ctx.Content.AsQueryable();
        return Mapper.Map<IEnumerable<ContentDetailsModel>>(res);
    }

    public bool AddTempContent(ContentDetailsModel contentDetailsModel)
    {
        var exist = Ctx.TempContent.FirstOrDefault(w => w.Tipus == contentDetailsModel.Tipus
            && (w.Cim.Contains(contentDetailsModel.Cim) || w.Nid == contentDetailsModel.Nid));
        if (exist is not null)
        {
            return false;
        }

        Ctx.TempContent.Add(Mapper.Map<TempContent>(contentDetailsModel));
        Ctx.SaveChanges();
        return true;
    }

    public bool AddVideo(VideoModel videoModel)
    {
        var exist = Ctx.VideoContent.FirstOrDefault(w => w.VideoLink == videoModel.VideoLink);
        if (exist is not null)
        {
            return false;
        }

        Ctx.VideoContent.Add(Mapper.Map<VideoContent>(videoModel));
        Ctx.SaveChanges();

        return true;
    }

    public bool DeleteVideoByNid(Guid nid)
    {
        var res = Ctx.VideoContent.FirstOrDefault(f => f.Nid == nid);
        if (res is null)
        {
            return false;
        }

        Ctx.VideoContent.Remove(res);
        Ctx.SaveChanges();
        return true;
    }

    public IEnumerable<VideoModel> GetVideoModels()
    {
        var res = Ctx.VideoContent.AsQueryable();
        return Mapper.Map<IEnumerable<VideoModel>>(res);
    }
}