using System.Net;

namespace ShopHttp.ShopHttpServer.HttpControllers
{
    public interface IHttpController
    {
        void StartController(HttpListenerContext context, string path);
    }
}
