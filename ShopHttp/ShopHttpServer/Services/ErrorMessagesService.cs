using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHttp.ShopHttpServer.Services
{
    internal class ErrorMessagesService
    {
        public ErrorMessagesService(string message)
        {
            IdNotFoundMessage = message;

        }


        public string IdNotFoundMessage { get; set; }
        public string EmptyMessage { get; set; }
    }
}
