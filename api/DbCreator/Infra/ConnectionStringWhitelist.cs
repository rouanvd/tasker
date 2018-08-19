using System.Linq;

namespace DbCreator.Infra
{
    static class ConnectionStringWhitelist
    {
        static string[] whitelist = new[] { @"SERVER=.", @"SERVER=.\SQL2008", @"SERVER=(LOCALDB)" };


        public static bool DbServerIsWhitelisted(string connectionStr)
        {
            if (string.IsNullOrWhiteSpace( connectionStr ))
                return false;

            bool isWhitelisted = whitelist.Any(whitelistedServer => connectionStr.ToUpper().Contains( whitelistedServer ));
            return isWhitelisted;
        }
    }
}
