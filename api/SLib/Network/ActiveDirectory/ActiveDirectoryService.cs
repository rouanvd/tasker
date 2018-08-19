using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Security.Authentication;

namespace SLib.Network.ActiveDirectory
{
    /// <summary>
    ///   Provides Active Directory (AD) routines so we can query AD for user information.
    /// </summary>
    public class ActiveDirectoryService
    {
        private readonly ActiveDirectoryConfig _config;


        //-------------------------------------------------------------------------------
        // PUBLIC (INTERFACE)
        //-------------------------------------------------------------------------------

        // ctor for dependency injection
        public ActiveDirectoryService(ActiveDirectoryConfig config)
        {
            _config = config;
        }


        /// <summary>
        ///   Finds all the Active Directory users that have a username, firstname, or surname that matches the
        ///   supplied searchPattern.  The searchPattern supports the '*' wildcard character.
        /// </summary>
        /// <param name="searchPattern">The search pattern to use to fined AD users.</param>
        public IList<ADUser> FindADUsers(string searchPattern)
        {
            DirectorySearcher adSearch = NewADUserSearch();

            // sets up the search filter to search on the user's: firstname, surname, username (without EMEA qualifier)
            adSearch.Filter = string.Format("(| (& (objectCategory=person)(objectClass=user)(givenname={0})) (& (objectCategory=person)(objectClass=user)(sn={0})) (& (objectCategory=person)(objectClass=user)(SAMAccountName={0})) )", searchPattern);

            SearchResultCollection adSearchResult = adSearch.FindAll();

            // convert the AD search result to a list of ADUser objects
            IList<ADUser> adUsers = new List<ADUser>();
            foreach (SearchResult result in adSearchResult)
                adUsers.Add(ToADUser(result));

            return adUsers;
        }


        /// <summary>
        ///   Gets the ADUser associated with the supplied loginName.
        /// </summary>
        /// <param name="loginName">The loginName to get the AD user details for in the format: 'EMEA\<username>'.</param>
        /// <param name="domain"></param>
        public ADUser GetADUserDetails(string loginName, string domain)
        {
            string userName = ExtractUsername(loginName);
            var search = NewADUserSearch(domain);

            search.Filter = String.Format("(SAMAccountName={0})", userName);

            SearchResult result;
            try
            {
                result = search.FindOne();

                if (result == null)
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

            ADUser adUser = ToADUser(result);
            return adUser;
        }


        /// <summary>
        ///   Checks if a user for the supplied loginName exists in Active Directory.
        /// </summary>
        /// <param name="loginName">Login name of user to check for existence.</param>
        public bool UserExistsInAD(string loginName)
        {
            //string userName = UserUtil.ExtractUsername(loginName);
            var search = new DirectorySearcher();

            search.Filter = String.Format("(SAMAccountName={0})", loginName);

            try
            {
                SearchResult result = search.FindOne();
                if (result == null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Gets all AD users.
        /// </summary>
        /// <returns></returns>
        public IList<ADUser> GetAllADUsers()
        {
            DirectorySearcher adSearch = NewADUserSearch();

            // sets up the search filter to find all users in AD
            adSearch.Filter = "(& (objectCategory=person)(objectClass=user) )";

            SearchResultCollection adSearchResult = adSearch.FindAll();

            // convert the AD search result to a list of ADUser objects
            IList<ADUser> adUsers = new List<ADUser>();
            foreach (SearchResult result in adSearchResult)
                adUsers.Add(ToADUser(result));

            return adUsers;
        }


        public IList<string> GetGroupsForADUser(string loginName, string domain)
        {
            Contract.Requires(! string.IsNullOrWhiteSpace(domain));

            try
            {
                var groupList = new List<string>();
                
                // PrincipalContext encapsulates the server or domain against which all operations are performed.            
                using (var pc = new PrincipalContext(ContextType.Domain, domain))
                {
                    // Create a reference to the user account we are querying against.
                    UserPrincipal up = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, loginName);            

                    if (up == null)
                        return new List<string>();
                
                    //Get the user's security groups.  This is necessary to return nested groups, but will NOT return distribution groups.
                    PrincipalSearchResult<Principal> groups = up.GetAuthorizationGroups();

                    // add each group to our groupList.  We have to use the Enumerator pattern to iterate over the elements in the
                    // groups collection and ignore any NoMatchingPrincipalException because of a _bug in ActiveDirectory's GetAuthorizationGroups()
                    // method.
                    // See: https://social.msdn.microsoft.com/Forums/vstudio/en-US/9dd81553-3539-4281-addd-3eb75e6e4d5d/getauthorizationgroups-fails-with-nomatchingprincipalexception?forum=csharpgeneral
                    var groupsEnumerator = groups.GetEnumerator();
                    while (groupsEnumerator.MoveNext())
                    {
                        try
                        {
                            var group = groupsEnumerator.Current;

                            if (group != null && ! string.IsNullOrWhiteSpace( group.SamAccountName ))
                                groupList.Add(group.SamAccountName);
                        }
                        catch (NoMatchingPrincipalException)
                        {
                        }                        
                    }
                }
            
                return groupList;
            }
            catch (PrincipalServerDownException ex)
            {
                Exception innerMostEx = ex.GetBaseException();
                throw new AuthenticationException("Could not authenticate against the Active Directory server.  The AD server could not be contacted.", innerMostEx);
            }
        }        


        ///// <summary>
        /////   Authenticates the supplied username/password credentials against the Active Directory store
        /////   and returns TRUE if the credentials are valid, or FALSE if they are not.  If the authentication failed,
        /////   this method throws an AuthenticateException.
        ///// </summary>
        ///// <param name="username">The username.</param>
        ///// <param name="password">The password.</param>
        ///// <param name="domain">The domain against which we will try to authenticate the credentials.</param>
        public bool Authenticate(string username, string password, string domain)
        {
            Contract.Requires(! string.IsNullOrWhiteSpace(username));
            Contract.Requires(! string.IsNullOrWhiteSpace(domain));

            string usernameWithoutDomainPrefix = ExtractUsername(username);

            try
            {
                using (var pc = new PrincipalContext(ContextType.Domain, domain))
                {
                    // validate the credentials
                    bool credentialsAreValid = pc.ValidateCredentials(usernameWithoutDomainPrefix, password);
                    return credentialsAreValid;
                }
            }
            catch (LdapException ex)
            {
                Exception innerMostEx = ex.GetBaseException();
                throw new AuthenticationException(innerMostEx.Message, innerMostEx);
            }
            catch (PrincipalServerDownException ex)
            {
                Exception innerMostEx = ex.GetBaseException();
                throw new AuthenticationException("Could not authenticate against the Active Directory server.  The AD server could not be contacted.", innerMostEx);
            }
        }


        //-------------------------------------------------------------------------------
        // PRIVATE (IMPLEMENTATION)
        //-------------------------------------------------------------------------------


        /// <summary>
        ///   Gets the value of the propertyName from the SearchResult, returning it as a string value.
        /// </summary>
        /// <param name="result">The search result to get the property value from.</param>
        /// <param name="propertyName">Name of the property whose value to retrieve from search result.</param>
        string GetPropertyValue(SearchResult result, string propertyName)
        {
            if (result.Properties[propertyName].Count >= 1)
                return (string)result.Properties[propertyName][0];
            return "";
        }


        /// <summary>
        ///   Creates a new, default configured search for searching for users in Active Directory, retrieving
        ///   standard user properties as part of the search result.
        /// </summary>
        /// <param name="domain">The domain to search against.  Defaults to ActiveDirectoryConfig.UserSearchDomain if no value is supplied.</param>
        DirectorySearcher NewADUserSearch(string domain = null)
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://"+ (domain ?? _config.UserSearchDomain));
            var search = new DirectorySearcher(de);

            // need to set the PageSize so we return more than 1000 objects (which is the default) if
            // no PageSize is set
            search.PageSize = 1000;

            // properties to load from AD for each user as part of the search results
            search.PropertiesToLoad.Add("samaccountname");
            search.PropertiesToLoad.Add("cn");
            search.PropertiesToLoad.Add("personalTitle");
            search.PropertiesToLoad.Add("givenname");
            search.PropertiesToLoad.Add("sn");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("userprincipalname");
            search.PropertiesToLoad.Add("l");
            search.PropertiesToLoad.Add("department");

            return search;
        }


        /// <summary>
        ///   Converts a single SearchResult object to an ADUser object.
        /// </summary>
        /// <param name="searchResult">The search result to convert.</param>
        ADUser ToADUser(SearchResult searchResult)
        {
            var adUser = new ADUser();

            adUser.Username          = GetPropertyValue(searchResult, "samaccountname");
            adUser.FullAccountName   = GetPropertyValue(searchResult, "cn");
            adUser.Title             = GetPropertyValue(searchResult, "personalTitle");
            adUser.Firstname         = GetPropertyValue(searchResult, "givenname");
            adUser.Surname           = GetPropertyValue(searchResult, "sn");
            adUser.Email             = GetPropertyValue(searchResult, "mail");
            adUser.UserPrincipalName = GetPropertyValue(searchResult, "userprincipalname");
            adUser.City              = GetPropertyValue(searchResult, "l");
            adUser.Department        = GetPropertyValue(searchResult, "department");

            return adUser;
        }


        /// <summary>
        ///   Returns the username portion of the supplied loginName.  A loginName has the following format:
        ///   'EMEA\<username>'.
        /// </summary>
        public static string ExtractUsername(string loginName)
        {
            string[] userPath = loginName.Split('\\');
            return userPath.Last();
        }
    }
}
