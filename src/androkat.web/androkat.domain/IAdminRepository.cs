#nullable enable
using androkat.domain.Model;
using androkat.domain.Model.AdminPage;
using androkat.domain.Model.WebResponse;
using System.Collections.Generic;
using System.Linq;

namespace androkat.domain;

public interface IAdminRepository
{
    AdminResult GetAdminResult();
    AdminBResult GetAdminBResult();
    (bool isSuccess, string? message) InsertIma(ImaModel imaModel);
    IEnumerable<AllTodayResult> LoadAllTodayResult();
    ContentResult? LoadPufferTodayContentByNid(string nid);
    LastTodayResult GetLastTodayContentByTipus(int tipus);
    bool InsertContent(ContentDetailsModel content);
    bool DeleteTempContentByNid(string nid);
    AdminAResult GetAdminAResult(bool isAdvent, bool isNagyBojt);
    IOrderedQueryable<AllRecordResult> GetAllContentByTipus(int tipus);   
    IOrderedQueryable<AllRecordResult> GetAllFixContentByTipus(int tipus); 
    ContentResult? LoadTodayContentByNid(string nid);
    ContentResult? LoadTodayFixContentByNid(string nid);
    bool DeleteContent(string nid);
    bool DeleteFixContent(string nid);
    bool UpdateContent(ContentDetailsModel content);
    bool UpdateFixContent(ContentDetailsModel content);
    bool LogInUser(string email);
    IEnumerable<AllRecordResult> GetAllMaiSzentByMonthResult(string date);
    ContentResult? LoadMaiSzentByNid(string nid);
    bool UpdateMaiSzent(MaiSzentModel maiszent);
    IEnumerable<AllRecordResult> GetAllImaByCsoportResult(string csoport);
    ContentResult? LoadImaByNid(string nid);
    bool UpdateIma(ImaModel imaModel);
    List<AllUserResult> GetUsers();
    bool UpdateRadio(RadioMusorModel radioMusorModel);
    IEnumerable<AllRecordResult> GetAllRadioResult();
    RadioResult? LoadRadioByNid(string nid);
    bool DeleteRadio(string nid);
    bool DeleteIma(string nid);
    IEnumerable<ImgData> GetImgList();
    IEnumerable<FileData> GetAudioList();
    Dictionary<int, string> GetAllContentTipusFromDb();
    Dictionary<int, string> GetAllFixContentTipusFromDb();
    bool InsertError(ErrorRequest content);
    List<SystemInfoData> GetIsAdventAndNagybojt();
	IEnumerable<SystemInfoData> GetAllSystemInfo();
	bool UpdateSystemInfo(SystemInfoData systemInfoData);
    (bool isSuccess, string? message) InsertFixContent(string cim, string idezet, int tipus, string datum);
}