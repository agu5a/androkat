using androkat.domain.Model;

namespace androkat.domain;

public interface IApiRepository
{
	bool AddContentDetailsModel(ContentDetailsModel contentDetailsModel);
        bool AddTempContent(ContentDetailsModel contentDetailsModel);
	bool UpdateRadioMusor(RadioMusorModel radioMusorModel);
}