namespace SLib.Network.ActiveDirectory
{
   public class ADUser
   {
        public string Username { get; set; }
        public string FullAccountName { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string UserPrincipalName { get; set; }
        public string City { get; set; }
        public string Department { get; set; }

        public string Fullname
        {
            get { return Firstname + " " + Surname; }
        }
    }
}
