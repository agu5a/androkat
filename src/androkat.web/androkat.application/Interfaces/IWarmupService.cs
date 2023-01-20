namespace androkat.application.Interfaces;

public interface IWarmupService
{
    void BookRadioSysCache();
    void ImaCacheFillUp();
    void MainCacheFillUp();
    void VideoCacheFillUp();
}