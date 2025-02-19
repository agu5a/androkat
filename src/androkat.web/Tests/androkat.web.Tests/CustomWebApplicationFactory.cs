using androkat.domain.Enum;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Model.SQLite;
using System;

namespace androkat.web.Tests;

public class CustomWebApplicationFactory<TStartup> : BaseWebApplicationFactory<TStartup> where TStartup : class
{
    public override void PopulateTestData(AndrokatContext dbContext)
    {
        dbContext.Content.RemoveRange(dbContext.Content);
        dbContext.RadioMusor.RemoveRange(dbContext.RadioMusor);
        dbContext.TempContent.RemoveRange(dbContext.TempContent);

        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");
        var yesterday = now.AddDays(-1).DateTime;
        var beforeOfyesterday = now.AddDays(-2).DateTime;

        dbContext.Content.Add(new Content
        {
            Nid = Guid.Parse("281cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.book,
            Cim = "cím1",
            Fulldatum = now.AddDays(-1).DateTime.ToString("yyyy-MM-dd"),
            Inserted = yesterday
        });
        dbContext.Content.Add(new Content
        {
            Nid = Guid.Parse("181cd115-1289-11ea-8aa1-cbeb38570c35"),
            Tipus = (int)Forras.b777,
            Cim = "cím2",
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
