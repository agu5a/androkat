using androkat.domain.Model.ContentCache;

namespace androkat.application.Interfaces;

public interface ICacheService
{
	ImaCache ImaCacheFillUp();
	MainCache MainCacheFillUp();
	VideoCache VideoCacheFillUp();
}