using System.Collections.Generic;

namespace SLib.Network.ActiveDirectory
{
    public class ActiveDirectoryConfig
    {
        public IEnumerable<string> LoginDomains {get;set;}
        public string UserSearchDomain {get;set;}
        public string ApplicationRolePrefix {get;set;}
    }
}