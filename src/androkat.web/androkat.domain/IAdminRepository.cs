using androkat.domain.Model;
using androkat.domain.Model.AdminPage;
using androkat.domain.Model.WebResponse;
using System.Collections.Generic;

namespace androkat.domain;

public interface IAdminRepository
{
    IEnumerable<AllTodayResult> LoadAllTodayResult();
    ContentResult LoadPufferNapiByNid(string nid);
    LastTodayResult GetLastTodayContentByTipus(int tipus);
    bool InsertContent(ContentDetailsModel content);
    bool DeleteTempContentByNid(string nid);
}