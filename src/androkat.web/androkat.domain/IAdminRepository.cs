using androkat.domain.Model;
using androkat.domain.Model.AdminPage;
using androkat.domain.Model.WebResponse;
using System.Collections.Generic;

namespace androkat.domain;

public interface IAdminRepository
{
    AdminResult GetAdminResult();
    AdminBResult GetAdminBResult();
    bool InsertIma(ImaModel imaModel);
    IEnumerable<AllTodayResult> LoadAllTodayResult();
    ContentResult LoadPufferTodayContentByNid(string nid);
    LastTodayResult GetLastTodayContentByTipus(int tipus);
    bool InsertContent(ContentDetailsModel content);
    bool DeleteTempContentByNid(string nid);
    AdminAResult GetAdminAResult(bool isAdvent, bool isNagyBojt);
    IEnumerable<AllRecordResult> GetAllContentByTipus(int tipus);
    ContentResult LoadTodayContentByNid(string nid);
    bool DeleteContent(string nid);
    bool UpdateContent(ContentDetailsModel content);
    bool LogInUser(string email);
    IEnumerable<AllRecordResult> GetAllMaiSzentByMonthResult(string date);
    ContentResult LoadMaiSzentByNid(string nid);
    bool UpdateMaiSzent(MaiSzentModel maiszent);
    IEnumerable<AllRecordResult> GetAllImaByCsoportResult(string csoport);
    ContentResult LoadImaByNid(string nid);
    bool UpdateIma(ImaModel imaModel);
    List<AllUserResult> GetUsers();
    bool UpdateRadio(RadioMusorModel radioMusorModel);
    IEnumerable<AllRecordResult> GetAllRadioResult();
    RadioResult LoadRadioByNid(string nid);
    bool DeleteRadio(string nid);
    bool DeleteIma(string nid);
    IEnumerable<ImgData> GetImgList();
    IEnumerable<FileData> GetAudioList();
    Dictionary<int, string> GetAllContentTipusFromDb();
    bool InsertError(ErrorRequest content);
    List<SystemInfoData> GetIsAdventAndNagybojt();
	IEnumerable<SystemInfoData> GetAllSystemInfo();
	bool UpdateSystemInfo(SystemInfoData systemInfoData);
}