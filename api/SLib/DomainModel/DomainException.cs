using System;

namespace SLib.DomainModel
{
    public class DomainException : Exception
    {
        public DomainException()
        {}


        public DomainException(string message)
        : base(message)
        {}
    }
}
