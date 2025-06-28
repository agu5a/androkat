using CommunityToolkit.Mvvm.ComponentModel;

namespace androkat.maui.library.ViewModels;

public partial class ContactViewModel : ViewModelBase
{
    public ContactViewModel()
    {
        Version = $"verzió: {AppInfo.VersionString}";
        Text1 = "<b>Célja</b>: Egyesíteni - amennyire lehet - a jelenleg elérhető katolikus mobil alkalmazások nyújtotta lehetőségeket és internetes szolgáltatásokat.<br />A napi imádság/elmélkedés támogatása de nem az idézetek gyűjtögetése.<br />Nem célunk, hogy boltban kapható könyveket ingyen letölthetővé tegyünk.<br />Ne feledjük azt, hogy '... a munkás megérdemli a maga bérét' (Lk 10,7), támogassuk a katolikus könyvkiadókat is.";
        Text2 = "<b>Fejlesztés, szerkesztés</b>:<br />Gulyás Arnold, Borsó Zsolt";
    }

    [ObservableProperty]
    string version;

    [ObservableProperty]
    string text1;

    [ObservableProperty]
    string text2;
}
