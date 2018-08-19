using SLib.DomainModel;

namespace Application.Authenz.Domain
{
    public class UserProfile : IUserProfile
    {
        public string Username {get;set;}
        public string FullName {get;set;}
    }
}
