﻿namespace androkat.maui.library.ViewModels;

public partial class ContactViewModel : ViewModelBase
{
    public ContactViewModel()
    {
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public string Version => AppInfo.VersionString;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public string Text1 => "<b>Célja</b>: Egyesíteni – amennyire lehet – a jelenleg elérhető katolikus mobil alkalmazások nyújtotta lehetőségeket és internetes szolgáltatásokat.<br />A napi imádság/elmélkedés támogatása de nem az idézetek gyűjtögetése.<br />Nem célunk, hogy boltban kapható könyveket ingyen letölthetővé tegyünk.<br />Ne feledjük azt, hogy '… a munkás megérdemli a maga bérét' (Lk 10,7), támogassuk a katolikus könyvkiadókat is.";

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public string Text2 => "<b>Fejlesztés, szerkesztés</b>:<br />Gulyás Arnold, Borsó Zsolt";

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public string Text3 => "<a href='https://www.facebook.com/androkat'>facebook</a>";

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public string Text4 => "<a href='mailto:uzenet@androkat.hu'>uzenet@androkat.hu</a>";

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public string Text5 => "<a href='https://twitter.com/AndroKat'>twitter</a>";

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public string Text6 => "<a href='https://www.youtube.com/channel/UCF3mEbdkhZwjQE8reJHm4sg'>youtube</a>";
}
