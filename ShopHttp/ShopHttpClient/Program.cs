using System.Net.Http;
using ShopHttp.ShopHttpClient.Services;
using ShopHttp.ShopHttpClient.Controllers;
using System;

namespace ShopHttp.ShopHttpClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var httpclient = new HttpClient();
            var baseUri = new Uri("http://localhost:44987/");
            var checkService = new CheckService();
            var productHttpController = new ProductHttpRequestController(httpclient, baseUri);
            var showcaseHttpController = new ShowcaseHttpRequestController(httpclient, baseUri);
            var productArchiveHttpController = new ProductArchiveHttpRequestController(httpclient, baseUri);
            var clientApplication = new ClientApplication(productHttpController, productArchiveHttpController, showcaseHttpController,
                                                             checkService);
            clientApplication.Run();
        }
    }
}
