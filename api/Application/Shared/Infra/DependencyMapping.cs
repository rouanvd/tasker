using Autofac;
using SLib.Network.ActiveDirectory;
using SLib.Network.Email;

namespace Application.Shared.Infra
{
    public static class DependencyMapping
    {
        public static void Initialize(ContainerBuilder builder)
        {
            builder.Register(c => new EfDbContext( ConfigReader.GetEfDbContextConnectionString() )).As<EfDbContext>().InstancePerLifetimeScope();

            builder.Register(c => ConfigReader.GetGeneralConfig()).As<AppConfig>();
            builder.Register(c => ConfigReader.GetEmailConfig()).As<EmailConfig>();
            builder.Register(c => ConfigReader.GetActiveDirectoryConfig()).As<ActiveDirectoryConfig>();
        }
    }
}
