using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.domain;

public interface IPartnerRepository
{
    bool DeleteTempContentByNid(string nid);
    ContentDetailsModel GetTempContentByNid(string nid);
    IEnumerable<ContentDetailsModel> GetTempContentByTipus(int tipus);
    bool LogInUser(string email);
    bool InsertTempContent(ContentDetailsModel contentDetailsModel);
}