using androkat.domain.Enum;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Model.SQLite;
using System;

namespace androkat.web.Tests;

public class ContentsWebApplicationFactory<TStartup> : BaseWebApplicationFactory<TStartup> where TStartup : class
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

        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");
        var yesterday = now.AddDays(-1).DateTime;
        var beforeOfyesterday = now.AddDays(-2).DateTime;

        dbContext.Content.Add(new Content
        {
            Nid = Guid.Parse("281cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.book,
            Cim = "cím1",
            Idezet = "idezet",
            Fulldatum = now.AddDays(-1).DateTime.ToString("yyyy-MM-dd"),
            Inserted = yesterday
        });
        dbContext.Content.Add(new Content
        {
            Nid = Guid.Parse("181cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.book,
            Cim = "cím2",
            Idezet = "idezet",
            Fulldatum = now.AddDays(-1).DateTime.ToString("yyyy-MM-dd"),
            Inserted = beforeOfyesterday
        });
        dbContext.Content.Add(new Content
        {
            Nid = Guid.Parse("381cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.b777,
            Cim = "cím1",
            Idezet = "idezet",
            Fulldatum = now.AddDays(-1).DateTime.ToString("yyyy-MM-dd"),
            Inserted = yesterday
        });
        dbContext.Content.Add(new Content
        {
            Nid = Guid.Parse("081cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.b777,
            Cim = "cím2",
            Idezet = "idezet",
            Fulldatum = now.AddDays(-1).DateTime.ToString("yyyy-MM-dd"),
            Inserted = beforeOfyesterday
        });       

        dbContext.SaveChanges();

        dbContext.TempContent.Add(new TempContent { Tipus = 1, Cim = "cím1" });
        dbContext.SaveChanges();

        dbContext.RadioMusor.Add(new RadioMusor { Source = "Source1", Musor = "Műsor1" });
        dbContext.SaveChanges();
    }
}
