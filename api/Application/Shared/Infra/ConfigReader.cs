using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SLib.Data;
using SLib.Network.ActiveDirectory;
using SLib.Network.Email;
using SLib.Prelude;

namespace Application.Shared.Infra
{
    public static class ConfigReader
    {
        public static AppConfig GetGeneralConfig()
        {
            var configRoot = (new ConfigurationBuilder())
                                .SetBasePath( Directory.GetCurrentDirectory() )
                                .AddJsonFile( "appsettings.json" )
                                .Build();

            // read the config settings from the appsettings.json file
            string environment                = configRoot.GetValue<string>("App.Environment")?.Trim();
            string restrictUsersTo            = configRoot.GetValue<string>("App.RestrictUsersTo")?.Trim();
            string enablePerformanceProfiling = configRoot.GetValue<string>("App.PerformanceProfiling.Enabled")?.Trim();
            string path_tempWorkingDir        = configRoot.GetValue<string>("Path.TempWorkingDir")?.Trim();

            var config = new AppConfig();
            config.Environment                = EnumModule.EnumFromString<RunningEnvironment>( environment ) ?? RunningEnvironment.Dev;
            config.RestrictedUsers            = restrictUsersTo?.Split(',').Select(s => s.Trim().ToUpper()).ToArray() ?? new string[0];
            config.EnablePerformanceProfiling = StringModule.ToBool( enablePerformanceProfiling, false );
            config.Path_TempWorkingDir        = path_tempWorkingDir.IfEmpty( @"C:\Temp\TempWorkingDir" );

            return config;
        }


        public static EmailConfig GetEmailConfig()
        {
            var configRoot = (new ConfigurationBuilder())
                                .SetBasePath( Directory.GetCurrentDirectory() )
                                .AddJsonFile( "appsettings.json" )
                                .Build();

            // read the config settings from the appsettings.json file
            string smtpHost    = configRoot.GetValue<string>("Email.SmtpHost")?.Trim();
            string smtpPort    = configRoot.GetValue<string>("Email.SmtpPort")?.Trim();
            string fromAddress = configRoot.GetValue<string>("Email.FromAddress")?.Trim();

            if (string.IsNullOrWhiteSpace(smtpHost)
                || string.IsNullOrWhiteSpace(smtpPort)
                || string.IsNullOrWhiteSpace(fromAddress))
            {
                throw new Exception("Could not read all the Email config parameters from the config file.");
            }

            // create the EmailSenderConfig object
            var config = new EmailConfig();
            config.SmtpHost    = smtpHost;
            config.SmtpPort    = StringModule.ToInt( smtpPort );
            config.FromAddress = fromAddress;

            return config;
        }


        public static ActiveDirectoryConfig GetActiveDirectoryConfig()
        {
            var configRoot = (new ConfigurationBuilder())
                                .SetBasePath( Directory.GetCurrentDirectory() )
                                .AddJsonFile( "appsettings.json" )
                                .Build();

            // read the config settings from the appsettings.json file
            string availableLoginADDomains = configRoot.GetValue<string>("ActiveDirectory.LoginDomains")?.Trim();
            string userSearchDomain        = configRoot.GetValue<string>("ActiveDirectory.UserSearchDomain")?.Trim();
            string adApplicationRolePrefix = configRoot.GetValue<string>("ActiveDirectory.ApplicationRolePrefix")?.Trim();

            if (string.IsNullOrWhiteSpace(availableLoginADDomains)
                || string.IsNullOrWhiteSpace(userSearchDomain)
                || string.IsNullOrWhiteSpace(adApplicationRolePrefix))
            {
                throw new Exception("Could not read all the Active Directory config parameters from the config file.");
            }

            // create the ActiveDirectoryConfig object, with its values read from the config file
            var config = new ActiveDirectoryConfig();
            config.LoginDomains          = availableLoginADDomains.Split(',').Select(s => s.Trim().ToUpper()).ToArray();
            config.UserSearchDomain      = userSearchDomain;
            config.ApplicationRolePrefix = adApplicationRolePrefix;

            return config;
        }


        public static string GetEfDbContextConnectionString()
        {
            var configRoot = (new ConfigurationBuilder())
                                .SetBasePath( Directory.GetCurrentDirectory() )
                                .AddJsonFile( "appsettings.json" )
                                .Build();

            var connectionStr = configRoot.GetConnectionString( "EfDbContext" );
            return connectionStr;
        }
    }
}
