using androkat.maui.library.Abstraction;
using androkat.maui.library.Data;
using androkat.maui.library.Helpers;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using androkat.maui.library.Models.Responses;

namespace androkat.maui.library.Services;

public class DownloadService : IDownloadService
{
    private readonly IRepository _repository;
    private readonly IAndrokatService _androkatService;
    private readonly IHelperSharedPreferences _helperSharedPreferences;
    private readonly ISourceData _sourceData;

    public DownloadService(IAndrokatService androkatService, IRepository repository,
        IHelperSharedPreferences helperSharedPreferences, ISourceData sourceData)
    {
        _repository = repository;
        _androkatService = androkatService;
        _helperSharedPreferences = helperSharedPreferences;
        _sourceData = sourceData;
    }

    private async Task DeleteOldItems()
    {
        if (!int.TryParse(_helperSharedPreferences.GetSharedPreferencesstring("maxOffline", ConsValues.defMaxOffline.ToString()), out int max))
        {
            max = ConsValues.defMaxOffline;
        }

        var res = await _repository.GetContentsWithoutBook();
        int dbCount = res.Count;

        if (dbCount > max)
        {
            int numb = dbCount - max;

            foreach (var item in res)
            {
                if (numb > 0)
                {
                    if (Enum.TryParse(item.TypeName, true, out Activities activities))
                    {
                        //fokolare-t nem töröljük az érintett hónapban
                        if (activities == Activities.fokolare)
                        {
                            if (IsSameAsToday(item.Datum, "MM"))
                                continue;
                        }

                        await _repository.DeleteContentByNid(item.Nid);
                        numb--;
                    }
                    else
                    {
                        await _repository.DeleteContentByNid(item.Nid);
                        numb--;
                    }
                }
                else
                    break;
            }
        }
    }

    public async Task<int> DownloadAll()
    {
        var fooldalResources = new List<Activities>
        {
            Activities.barsi,
            Activities.horvath,
            Activities.bojte,
            Activities.kempis,
            Activities.advent,
            Activities.nagybojt,
            Activities.maievangelium,// "yyyy-MM-dd"*/
            Activities.prohaszka,
            Activities.taize,
            Activities.regnum,
            Activities.papaitwitter,
            Activities.fokolare,// "MM"
            Activities.szeretetujsag,
            Activities.laciatya,

            Activities.maiszent,

            Activities.humor,

            Activities.prayasyougo,
            Activities.audiobarsi,
            Activities.audiohorvath,
            Activities.audiopalferi,
            Activities.audiotaize,
            Activities.audionapievangelium,

            Activities.book,

            Activities.ajanlatweb,

            Activities.kurir,
            Activities.bonumtv,
            Activities.keresztenyelet,

            Activities.b777,
            Activities.bkatolikusma,
            Activities.jezsuitablog,

            Activities.ignac,
            Activities.pio,
            Activities.janospal,
            Activities.sztjanos,
            Activities.kisterez,
            Activities.vianney,
            Activities.terezanya,
            Activities.szentbernat,
            Activities.szentszalezi,
            Activities.sienaikatalin,

            Activities.group_ima
        };

        var count = 0;
        foreach (var activity in fooldalResources)
        {
            int result = await StartUpdate(activity);
            if (result == -1) //nem érhető el az androkat.hu
                return -1;

            count += result;
        }

        await DeleteOldItems();

        return count;
    }

    public async Task<int> StartUpdate(Activities act)
    {
        try
        {
            if (act == Activities.group_ima)
            {
                return await InsertIma(0);
            }

            //a mai szent-et, ajanlatokat, humort nem lehet letiltani a beallitásokban, így mindig ellenőrizzük
            if (act != Activities.maiszent && act != Activities.ajanlatweb
                && act != Activities.humor
                && !_helperSharedPreferences.GetSharedPreferencesBoolean(act.ToString(), true))// A tobbit csak ha keri a beállításokban
                return 0;

            int result = 0;
            var latestItem = await HasTodayData(act);

            //a news, blog és book mindig lekérdezve a szerverről az utolsó nid-del
            if (act == Activities.kurir
                    || act == Activities.bonumtv
                    || act == Activities.keresztenyelet
                    || act == Activities.b777
                    || act == Activities.bkatolikusma
                    || act == Activities.jezsuitablog
                    || act == Activities.book)
            {
                result = await DownloadData(latestItem.nid.ToString(), act);
            }
            else
            {
                //csak ha még nincs meg az aznapi
                if (!latestItem.exists)
                    result = await DownloadData(latestItem.nid.ToString(), act);
            }

            return result; // -1: nem érhető el az androkat.hu

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** StartUpdate EXCEPTION! {ex}");
        }

        return 0;
    }

    private async Task<int> InsertIma(int imak)
    {
        try
        {
            DateTime datum = (await GetLatestFromLocalDb(Activities.group_ima)).datum;

            ImaResponse result = await GetImaFromServer(datum);
            List<ImadsagResponse> listSync = result.Imak;

            if (listSync.Count > 0)
            {
                foreach (ImadsagResponse imadsag in listSync)
                {
                    //TODO törölni, ha lesz fix API
                    await _repository.DeleteImadsagByNid(imadsag.Nid);

                    if (datum > DateTime.MinValue && datum < imadsag.RecordDate || datum == DateTime.MinValue)
                    {
                        await _repository.InsertImadsag(new ImadsagEntity
                        {
                            Cim = imadsag.Title.Replace("<b>", "").Replace("</b>", ""),
                            Content = imadsag.Content,
                            Datum = imadsag.RecordDate,
                            TypeName = Activities.ima.ToString(),
                            IsRead = 0,
                            IsHided = false,
                            GroupName = "group_ima",
                            Nid = imadsag.Nid,
                            Csoport = imadsag.Csoport
                        });

                        imak++;
                    }
                }

                if (result.HasMore)
                    imak = await InsertIma(imak);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** InsertIma EXCEPTION! {ex}");
        }

        return imak;
    }

    private async Task<(Guid nid, DateTime datum)> GetLatestFromLocalDb(Activities type)
    {
        try
        {
            if (type == Activities.group_ima)
            {
                ImadsagEntity imadsag = await _repository.GetFirstImadsag();
                if (imadsag != null)
                {
                    return (imadsag.Nid, imadsag.Datum);
                }
            }
            else
            {
                ContentEntity idezet = await _repository.GetContentsByTypeName(type.ToString());
                if (idezet != null)
                    return (idezet.Nid, idezet.Datum);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** GetLatestFromLocalDb EXCEPTION! {ex}");
        }

        return (Guid.Empty, DateTime.MinValue);
    }

    private async Task<(Guid nid, bool exists)> HasTodayData(Activities type)
    {
        var latestItem = await GetLatestFromLocalDb(type);

        try
        {
            if (latestItem.datum == DateTime.MinValue)//nincs a db-ben adat
                return (Guid.Empty, false);

            bool isHasTodayData = false;

            if (type == Activities.maievangelium)
            {
                //ezt mindig lekérjük, mert szombaton megjön a vasárnapi ige is
                return (Guid.Empty, false);
            }
            else if (type == Activities.maiszent)
            {
                isHasTodayData = IsSameAsToday(latestItem.datum, "MMdd");
            }
            else if (type == Activities.fokolare)
            {
                isHasTodayData = IsSameAsToday(latestItem.datum, "MM");
            }
            else if (type != Activities.kurir
                    && type != Activities.bonumtv
                    && type != Activities.keresztenyelet
                    && type != Activities.b777
                    && type != Activities.bkatolikusma
                    && type != Activities.jezsuitablog
                    && type != Activities.book)
                isHasTodayData = IsSameAsToday(latestItem.datum, "MM-dd");

            if (isHasTodayData)
                return (latestItem.nid, true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** HasTodayData EXCEPTION! {ex}");
        }

        return (latestItem.nid, false);
    }

    private async Task<ImaResponse> GetImaFromServer(DateTime date)
    {
        var result = new ImaResponse();
        List<ImadsagResponse> list = new();
        result.Imak = list;
        result.HasMore = false;

        try
        {
            ImaResponse response = await _androkatService.GetImadsag(date);
            if (response == null)
                return result;

            for (int i = 0; i < response.Imak.Count; i++)
            {
                try
                {
                    var recordDate = response.Imak[i].RecordDate;
                    var title = Helper.ReplaceUtf8(response.Imak[i].Title);
                    var content = Helper.ReplaceUtf8(response.Imak[i].Content);
                    var nid = response.Imak[i].Nid;
                    var csoport = response.Imak[i].Csoport;

                    ImadsagResponse imadsag = new();
                    imadsag.Title = title.Replace("<b>", "").Replace("</b>", "");
                    imadsag.Content = content;
                    imadsag.RecordDate = recordDate;
                    imadsag.Nid = nid;
                    imadsag.Csoport = csoport;
                    list.Add(imadsag);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"********************************** GetImaFromServer EXCEPTION! {ex}");
                }
            }

            result.HasMore = response.HasMore;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** GetImaFromServer EXCEPTION! {ex}");
        }

        return result;
    }

    private async Task<int> DownloadData(string nid, Activities tipusName)
    {
        //var debug = await conn.Table<ContentEntity>().ToListAsync();
        int tipusId = (int)tipusName;
        string tipus = tipusId.ToString();

        try
        {
            var res = await _androkatService.GetContents(tipus, nid);
            SourceData idezetSource = _sourceData.GetSourcesFromMemory(tipusId);
            var count = 0;
            foreach (var item in res)
            {
                try
                {
                    await _repository.InsertContent(new ContentEntity
                    {
                        Cim = Helper.ReplaceUtf8(item.Cim.Replace("<b>", "").Replace("</b>", "")),
                        Nid = item.Nid,
                        Datum = item.Datum,
                        Forras = item.Forras,
                        Idezet = Helper.ReplaceUtf8(item.Idezet),
                        Image = item.Image,
                        Tipus = tipus,
                        TypeName = tipusName.ToString(),
                        IsRead = false,
                        KulsoLink = Helper.ReplaceUtf8(item.KulsoLink),
                        GroupName = idezetSource.GroupName
                    });
                    count++;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"********************************** DownloadData InsertAsync ContentEntity EXCEPTION! {ex}");
                }
            }

            return count;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** DownloadData EXCEPTION! {ex}");
        }

        return -1;
    }

    private static bool IsSameAsToday(DateTime dateInput, string format)
    {
        // format egyezosseg, ha egyezik nem kell hivni a kulso service-t
        return dateInput.ToString(format) == DateTime.UtcNow.ToString(format);
    }
}
