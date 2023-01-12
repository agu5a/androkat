namespace androkat.application.Interfaces;

public interface IWarmupService
{
    void BookRadioSysCache();
    void EgyebCacheFillUp();
	void ImaCacheFillUp();
	void MainCacheFillUp();
	void VideoCacheFillUp();
}