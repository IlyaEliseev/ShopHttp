﻿using System;

namespace ShopHttp.ShopHttpServer.Services
{
    public class EmptyCollectionException : Exception
    {
        public EmptyCollectionException()
        {

        }

        public EmptyCollectionException(string message) : base(message)
        {

        }

        public EmptyCollectionException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
