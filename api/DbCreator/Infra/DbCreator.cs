using System;
using Application.Authenz.Domain;
using Application.Shared.Infra;
using DbCreator.Infra._Internal;
using DbCreator.Initializers;
using Microsoft.EntityFrameworkCore;
using SLib.DomainModel;

namespace DbCreator.Infra
{
    class DbCreator
    {
        public void CreateDb(CreateDbOptions options)
        {
            string connectionString = GetConnectionString();

            // confirm that the user wants to continue to protect against accidental running of DbCreator in QA or PROD.
            if (! UserConfirmedDbCreatorAction( connectionString ))
            {
                WriteLine();
                WriteLine( ">> No action was performed because user did NOT confirm action." );
                WriteLine( "Press ENTER to close DbCreator." );
                Console.ReadLine();

                return;
            }

            var dbModule = new DbModule(connectionString);

            dbModule.KillActiveConnections();

            if (options.DropDb)
                dbModule.DropDatabase();

            CreateDatabaseAndSchema();

            if (options.LoadSeedData)
                LoadSeedData();

            if (options.LoadTestData)
                LoadTestData();
        }


        string GetConnectionString()
        {
            using (var dbCtx = new EfDbContext( ConfigReader.GetEfDbContextConnectionString() ))
            {
                string connectionStr = dbCtx.Database.GetDbConnection().ConnectionString;

                if (string.IsNullOrWhiteSpace(connectionStr))
                    throw new Exception("No connection string defined for Entity Framework DbContext.  Make sure a connection string is defined in the config file.");

                return connectionStr;
            }
        }


        void CreateDatabaseAndSchema()
        {
            using (var dbCtx = new EfDbContext( ConfigReader.GetEfDbContextConnectionString() ))
            {
                dbCtx.Database.EnsureCreated();
            }
        }


        void LoadSeedData()
        {
            var userProfile = GetDbCreatorUserProfile();
            using (var dbCtx = new EfDbContext(ConfigReader.GetEfDbContextConnectionString(), userProfile))
            {
                DbSeedDataInitialiser.Init(dbCtx);
            }
        }


        void LoadTestData()
        {
            var userProfile = GetDbCreatorUserProfile();
            using (var dbCtx = new EfDbContext(ConfigReader.GetEfDbContextConnectionString(), userProfile))
            {
                DbTestDataInitialiser.Init(dbCtx);
            }            
        }


        IUserProfile GetDbCreatorUserProfile()
        {
            var userProfile = new UserProfile { FullName = "DbCreator", Username = "DbCreator" };
            return userProfile;
        }


        bool UserConfirmedDbCreatorAction(string connectionString)
        {
            // before executing the DbCreator action, we display the connection string so that the user can confirm that he is
            // looking at the correct server before executing potentially dangerous actions on the DB.
            WriteLine( "Connecting to: "+ connectionString );
            if (! PromptUserToContinue())
                return false;

            // after the user confirmed the first time, we do an additional check to see if the server pointed to in the 
            // connection string is in our whitelist.  This is to protect us from accidentally running DbCreator against
            // QA or PROD.
            if (! ConnectionStringWhitelist.DbServerIsWhitelisted( connectionString ))
            {
                WriteLine();
                WriteLine( "WARNING: The server specified in the connection string is NOT *white listed* and is not allowed.", ConsoleColor.Yellow );

                if (! PromptUserToContinue())
                    return false;
            }

            return true;
        }


        void WriteLine(string message = null, ConsoleColor? textColor = null)
        {
            ConsoleColor originalColor = Console.ForegroundColor;

            if (textColor.HasValue)
                Console.ForegroundColor = textColor.Value;

            Console.WriteLine( message );

            Console.ForegroundColor = originalColor;
        }


        bool PromptUserToContinue()
        {
            Console.Write( "Continue (yes|no): " );
            string userInput = Console.ReadLine();

            bool userWantsToContinue = userInput != null && userInput.ToUpper() == "YES";
            return userWantsToContinue;
        }
    }


    public class CreateDbOptions
    {
        public bool DropDb {get;set;}       = false;
        public bool LoadSeedData {get;set;} = false;
        public bool LoadTestData {get;set;} = false;
    }
}
