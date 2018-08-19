namespace SLib.Network.Email
{
    /// <summary>
    ///   Class that encapsulates the general config settings for the sending Email.
    /// </summary>
    public class EmailConfig
    {
        public string SmtpHost {get;set;}
        public int SmtpPort {get;set;}
        public string FromAddress {get;set;}


        public EmailConfig()
        {
            SmtpHost    = "";
            SmtpPort    = 25;
            FromAddress = "";
        }
    }
}
