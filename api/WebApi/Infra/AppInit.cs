using System;
using Application.Shared.Infra;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Infra
{
    public static class AppInit
    {
        public static IServiceProvider InitAutofac(IServiceCollection services)
        {
            // Create an Autofac Container and push the framework services
            var containerBuilder = new ContainerBuilder();            
            containerBuilder.Populate(services);
         
            // Register your own services within Autofac
            DependencyMapping.Initialize( containerBuilder );
         
            // Build the container and return an IServiceProvider from Autofac
            var container = containerBuilder.Build();
            return container.Resolve<IServiceProvider>();    
        }
    }
}
