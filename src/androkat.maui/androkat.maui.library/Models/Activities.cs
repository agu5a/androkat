namespace androkat.maui.library.Models;

public enum Activities
{
    noone = 0,
    //napi olvaso
    mello = 1, pio = 2, janospal = 3, sztjanos = 4, kisterez = 5, audiohorvath = 6, fokolare = 7, terezanya = 8, papaitwitter = 9, advent = 10,
    maievangelium = 11, ignac = 12, barsi = 13, horvath = 14, prayasyougo = 15, nagybojt = 16, bojte = 17,
    //news
    kurir = 18, kempis = 19, bonumtv = 20,

    maiszent = 21,//group is és typeName is

    favorite = 22,

    ima = 23, szeretetujsag = 24, humor = 25,

    contact = 26,

    group_humor = 27,//group
    audionapievangelium = 28,
    keresztenyelet = 29,
    igenaptar = 30,
    radio = 31,
    video = 32,
    settings = 33,
    gyonas = 34,

    //groupok, mikor nem egyesevel adott screen tipusai
    group_napiolvaso = 35, group_news = 36, group_ima = 37,

    audiobarsi = 38, audiopalferi = 39,

    group_audio = 40, //group

    torlesre_var = 41,
    vianney = 42,//napi olvaso

    //magazin
    b777 = 43,
    group_blog = 44,//group

    regnum = 45,//napi olvaso
    book = 46, //group is és typeName is
    taize = 47,//napi olvaso
    prohaszka = 48,//napi olvaso
    bzarandokma = 49,//magazin
    szentbernat = 50,//napi olvaso
    jezsuitablog = 51,//magazin

    szentszalezi = 52,//napi olvaso
    group_szentek = 53,//group
    keresztut = 54,
    szentter = 55,
    refresh = 56,
    laciatya = 57,
    ajanlatweb = 58,
    group_ajanlatok = 59, //group
    audiotaize = 60,
    sienaikatalin = 61,
    medjugorje = 62,
    group_web = 63,
    martonaron = 64
}

public static class ActivitiesHelper
{
    public static Activities GetActivitiesByValue(int value)
    {
        try
        {
            Activities activity = (Activities)value;
            return activity;
        }
        catch (Exception)
        {
            // ignored
        }

        return Activities.noone;
    }
}