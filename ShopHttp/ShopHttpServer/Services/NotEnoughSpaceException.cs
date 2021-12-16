using System;

namespace ShopHttp.ShopHttpServer.Services
{
    public class NotEnoughSpaceException : Exception
    {
        public NotEnoughSpaceException() 
        {

        }

        public NotEnoughSpaceException(string message) : base(message)
        {

        }

        public NotEnoughSpaceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
