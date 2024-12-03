using androkat.domain.Enum;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Model.SQLite;
using System;

namespace androkat.web.Tests;

public class ContentsTodayWebApplicationFactory<TStartup> : BaseWebApplicationFactory<TStartup> where TStartup : class
{
    public override void PopulateTestData(AndrokatContext dbContext)
    {
        foreach (var item in dbContext.Content)
        {
            dbContext.Remove(item);
        }

        foreach (var item in dbContext.RadioMusor)
        {
            dbContext.Remove(item);
        }

        var now = DateTime.Now;
        var yesterday = now.AddDays(-1);
        var beforeOfyesterday = now.AddDays(-2);       

        dbContext.Content.Add(new Content
        {
            Nid = Guid.Parse("a81cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.ajanlatweb,
            Cim = "cím1",
            Idezet = "idezet",
            Fulldatum = now.ToString("yyyy-MM-dd"),
            Inserted = yesterday
        });
        dbContext.Content.Add(new Content
        {
            Nid = Guid.Parse("a71cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.ajanlatweb,
            Cim = "cím2",
            Idezet = "idezet",
            Fulldatum = now.AddDays(-1).ToString("yyyy-MM-dd"),
            Inserted = beforeOfyesterday
        });

        dbContext.FixContent.Add(new FixContent
        {
            Nid = Guid.Parse("b81cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.humor,
            Cim = "cím1",
            Idezet = "idezet",
            Datum = now.ToString("MM-dd")
        });
        dbContext.FixContent.Add(new FixContent
        {
            Nid = Guid.Parse("b71cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.humor,
            Cim = "cím2",
            Idezet = "idezet",
            Datum = now.AddDays(-1).ToString("MM-dd")
        });

        dbContext.FixContent.Add(new FixContent
        {
            Nid = Guid.Parse("c81cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.pio,
            Cim = "cím1",
            Idezet = "idezet",
            Datum = now.ToString("MM-dd")
        });
        dbContext.FixContent.Add(new FixContent
        {
            Nid = Guid.Parse("c71cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.pio,
            Cim = "cím2",
            Idezet = "idezet",
            Datum = now.AddDays(-1).ToString("MM-dd")
        });

        dbContext.MaiSzent.Add(new Maiszent
        {
            Nid = Guid.Parse("e81cd115-1289-11ea-8aa1-cbeb38570c35"),
            Cim = "cím1",
            Idezet = "idezet",
            Datum = now.ToString("MM-dd"),
            Inserted = yesterday
        });
        dbContext.MaiSzent.Add(new Maiszent
        {
            Nid = Guid.Parse("e71cd115-1289-11ea-8aa1-cbeb38570c35"),
            Cim = "cím2",
            Idezet = "idezet",
            Datum = now.AddDays(-1).ToString("MM-dd"),
            Inserted = beforeOfyesterday
        });

        dbContext.Content.Add(new Content
        {
            Nid = Guid.Parse("f81cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.audiotaize,
            Cim = "cím1",
            Idezet = "audiofile",
            Fulldatum = now.ToString("yyyy-MM-dd"),
            Inserted = yesterday
        });
        dbContext.Content.Add(new Content
        {
            Nid = Guid.Parse("f71cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.audiotaize,
            Cim = "cím2",
            Idezet = "audiofile",
            Fulldatum = now.AddDays(-1).ToString("yyyy-MM-dd"),
            Inserted = beforeOfyesterday
        });

        dbContext.Content.Add(new Content
        {
            Nid = Guid.Parse("dd1cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.audionapievangelium,
            Cim = "cím1",
            Idezet = "audiofile",
            Fulldatum = now.ToString("yyyy-MM-dd"),
            Inserted = yesterday
        });
        dbContext.Content.Add(new Content
        {
            Nid = Guid.Parse("d7dcd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.audionapievangelium,
            Cim = "cím2",
            Idezet = "audiofile",
            Fulldatum = now.AddDays(-1).ToString("yyyy-MM-dd"),
            Inserted = beforeOfyesterday
        });

        dbContext.SaveChanges();

        dbContext.TempContent.Add(new TempContent { Tipus = 1, Cim = "cím1" });
        dbContext.SaveChanges();

        dbContext.RadioMusor.Add(new RadioMusor { Source = "Source1", Musor = "Műsor1" });
        dbContext.SaveChanges();
    }
}
