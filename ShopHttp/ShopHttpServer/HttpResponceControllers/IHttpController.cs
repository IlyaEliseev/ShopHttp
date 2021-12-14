using System.Net;

namespace ShopHttp.ShopHttpServer.HttpResponceControllers
{
    public interface IHttpController
    {
        void StartController(HttpListenerContext context, string path);
    }
}
