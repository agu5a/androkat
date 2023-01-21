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
        builder.RegisterType<ApiService>().As<IApiService>().InstancePerLifetimeScope();
        builder.RegisterDecorator<ApiServiceCacheDecorate, IApiService>();
        builder.RegisterType<WarmupService>().As<IWarmupService>().InstancePerLifetimeScope();
        builder.RegisterType<ContentService>().As<IContentService>().InstancePerLifetimeScope();
    }
}
