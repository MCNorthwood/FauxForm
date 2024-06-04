using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;

namespace FauxForm.API.Infrastructure.Extensions;

public static class AutoFacExtensions
{
    public static IHostBuilder AddAutoFacServices(this ConfigureHostBuilder host)
    {
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>((container) =>
            {
                container.RegisterAssemblyTypes(Assembly.Load("FauxForm.Engine.Repository")).AsImplementedInterfaces();
                container.RegisterAssemblyTypes(Assembly.Load("FauxForm.Engine.Services")).AsImplementedInterfaces();
                container.RegisterAssemblyTypes(Assembly.Load("FauxForm.Engine.Mapping")).AsImplementedInterfaces();
                container.RegisterAssemblyTypes(Assembly.Load("FauxForm.Engine.Validation")).AsImplementedInterfaces();
            });

        return host;
    }
}