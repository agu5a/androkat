using androkat.application.Interfaces;
using androkat.application.Service;
using Autofac;
using System.Diagnostics.CodeAnalysis;

namespace androkat.application.DI;

[ExcludeFromCodeCoverage]
public class DefaultCoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ContentService>().As<IContentService>().InstancePerLifetimeScope();
    }
}
