using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using SLib.FileSystem;

namespace DbCreator.Infra._Internal
{
    class DbModule
    {
        const string KILL_ACTIVE_CONNECTIONS_SQL = "DbCreator.SqlScripts.KillActiveConnections.sql";
        string _connectionString = null;


        public DbModule(string connectionString)
        {
            _connectionString = connectionString;
        }


        public void KillActiveConnections()
        {
            if (! DatabaseExists())
                return;

            WithSqlServerDo((dbServer, dbName) =>
            {
                string sql = GetKillAllActiveConnectionsSql();
                dbServer.ConnectionContext.DatabaseName = dbName;
                dbServer.ConnectionContext.ExecuteNonQuery( sql );
            });
        }


        public void DropDatabase()
        {
            if (! DatabaseExists())
                return;

            WithSqlServerDo((dbServer, dbName) =>
            {
                dbServer.ConnectionContext.ExecuteNonQuery($"DROP DATABASE [{dbName}]");
            });
        }


        public void CreateDatabase()
        {
            if (DatabaseExists())
                return;

            WithSqlServerDo((dbServer, dbName) =>
            {
                dbServer.ConnectionContext.ExecuteNonQuery($"CREATE DATABASE [{dbName}]");
            });
        }


        public void ExecuteSqlScriptOnServer(string sqlScriptResourceName)
        {
            var sqlScriptStream = GetManifestResourceStream(sqlScriptResourceName);
            using (var sqlScriptStreamReader = new StreamReader(sqlScriptStream))
            {
                string script = sqlScriptStreamReader.ReadToEnd();

                WithSqlServerDo((dbServer, dbName) =>
                {
                    // We use Sql Management Objects (SMO) here because scripts may contain batches of Sql statements separated by 'GO' statements.
                    // As per http://stackoverflow.com/questions/3701147/in-sql-server-when-should-you-use-go-and-when-should-you-use-semi-colon
                    // 'GO' is not actually part of the T-SQL language, it is a feature of Sql Management Programs such as Sql Management Studio 
                    // As per http://stackoverflow.com/questions/5479695/elmah-ddl-giving-sql-errors-when-run-by-context-database-executesqlcommand-ef4
                    // currently the best way to handle scripts (specifically the elmah ddl script), with Entity Framework, is to use SMO.
                    dbServer.ConnectionContext.ExecuteNonQuery( script );
                });
            }
        }


        public void ExecuteSqlScriptOnDb(string sqlScriptResourceName)
        {
            var sqlScriptStream = GetManifestResourceStream(sqlScriptResourceName);
            using (var sqlScriptStreamReader = new StreamReader(sqlScriptStream))
            {
                string script = sqlScriptStreamReader.ReadToEnd();

                WithSqlServerDo((dbServer, dbName) =>
                {
                    // always add a 'USE $DB;' statement when running scripts that should be for a specific DB so that the
                    // developer does not need to remember to do this in the SQL script
                    var queries = new StringCollection();
                    queries.Add( $"USE {dbName};" );
                    queries.Add( script );

                    // We use Sql Management Objects (SMO) here because scripts may contain batches of Sql statements separated by 'GO' statements.
                    // As per http://stackoverflow.com/questions/3701147/in-sql-server-when-should-you-use-go-and-when-should-you-use-semi-colon
                    // 'GO' is not actually part of the T-SQL language, it is a feature of Sql Management Programs such as Sql Management Studio 
                    // As per http://stackoverflow.com/questions/5479695/elmah-ddl-giving-sql-errors-when-run-by-context-database-executesqlcommand-ef4
                    // currently the best way to handle scripts (specifically the elmah ddl script), with Entity Framework, is to use SMO.
                    dbServer.ConnectionContext.ExecuteNonQuery( queries );
                });
            }
        }


        public void ExecuteSqlOnDb(string sql)
        {
            WithSqlServerDo((dbServer, dbName) =>
            {
                // always add a 'USE $DB;' statement when running scripts that should be for a specific DB so that the
                // developer does not need to remember to do this in the SQL script
                var queries = new StringCollection();
                queries.Add( $"USE {dbName};" );
                queries.Add( sql );

                // We use Sql Management Objects (SMO) here because scripts may contain batches of Sql statements separated by 'GO' statements.
                // As per http://stackoverflow.com/questions/3701147/in-sql-server-when-should-you-use-go-and-when-should-you-use-semi-colon
                // 'GO' is not actually part of the T-SQL language, it is a feature of Sql Management Programs such as Sql Management Studio 
                // As per http://stackoverflow.com/questions/5479695/elmah-ddl-giving-sql-errors-when-run-by-context-database-executesqlcommand-ef4
                // currently the best way to handle scripts (specifically the elmah ddl script), with Entity Framework, is to use SMO.
                dbServer.ConnectionContext.ExecuteNonQuery( queries );
            });
        }


        bool DatabaseExists()
        {
            bool dbExists = false;
            WithSqlServerDo((dbServer, dbName) => dbExists = dbServer.Databases.Contains(dbName));

            return dbExists;
        }


        void WithSqlServerDo(Action<Server, string> action)
        {
            var dbConnection = new SqlConnection(_connectionString);
            var dbServer = new Server(new ServerConnection(dbConnection.DataSource));

            action(dbServer, dbConnection.Database);

            dbServer.ConnectionContext.Disconnect();
        }


        static string GetKillAllActiveConnectionsSql()
        {
            string sql = EmbeddedResourcesModule.GetEmbeddedResourceAsText(typeof(DbModule).Assembly, KILL_ACTIVE_CONNECTIONS_SQL);
            return sql;
        }


        Stream GetManifestResourceStream(string resourceName)
        {
            Assembly assembly = typeof(DbModule).Assembly;
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new Exception($"Could not locate resource file: {resourceName}");

            return stream;
        }
    }
}
