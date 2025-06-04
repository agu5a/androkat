using androkat.infrastructure.Model.SQLite;
using AutoFixture;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Tests.Attributes;

public class AutoDataWithCustomData : AutoDataAttribute
{
    public AutoDataWithCustomData()
        : base(() =>
        {
            var fixture = new Fixture();

            fixture.Customize<Content>(composer =>
            composer.With(x => x.Cim, "Cím")
            .With(x => x.Nid, () => Guid.NewGuid())
            .With(x => x.Fulldatum, DateTime.Now.ToString("yyyy") + "-02-03"));

            fixture.Customize<FixContent>(composer =>
            composer.With(x => x.Cim, "Cím")
            .With(x => x.Nid, () => Guid.NewGuid())
            .With(x => x.Datum, "02-03"));

            fixture.Customize<ImaContent>(composer =>
            composer.With(x => x.Cim, "Cím")
            .With(x => x.Nid, () => Guid.NewGuid())
            .With(x => x.Datum, DateTime.Now)
            .With(x => x.Csoport, "1"));

            // Configure to return exactly 2 Content instances when List<Content> is requested
            fixture.Customize<List<Content>>(composer =>
                composer.FromFactory(() => fixture.CreateMany<Content>(2).ToList()));

            return fixture;
        })
    {
    }
}