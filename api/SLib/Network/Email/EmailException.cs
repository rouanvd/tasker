using System;

namespace SLib.Network.Email
{
    public class EmailException: Exception
    {
        public EmailException(string message) : base(message)
        {            
        }
    }
}
