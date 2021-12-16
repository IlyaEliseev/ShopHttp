using System;

namespace ShopHttp.ShopHttpServer.Services
{
    public class NotEmptyCollectionException : Exception
    {
        public NotEmptyCollectionException()
        {

        }

        public NotEmptyCollectionException(string message) : base(message)
        {

        }

        public NotEmptyCollectionException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
