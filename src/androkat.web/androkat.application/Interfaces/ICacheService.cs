using androkat.domain.Model.ContentCache;

namespace androkat.application.Interfaces;

public interface ICacheService
{
    BookRadioSysCache BookRadioSysCacheFillUp();
	ImaCache ImaCacheFillUp();
	MainCache MainCacheFillUp();
	VideoCache VideoCacheFillUp();
}