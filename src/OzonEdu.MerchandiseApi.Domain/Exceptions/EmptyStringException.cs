using System;

namespace OzonEdu.MerchandiseApi.Domain.Exceptions
{
    public class EmptyStringException : Exception
    {
        public EmptyStringException(string message) : base(message)
        { }

        public EmptyStringException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}