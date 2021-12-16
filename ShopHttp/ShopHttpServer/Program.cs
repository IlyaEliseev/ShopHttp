using ShopHttp.ShopHttpServer.Controllers;
using ShopHttp.ShopHttpServer.HttpResponceControllers;
using ShopHttp.ShopHttpServer.Services;
using System.Net;

namespace ShopHttp.ShopHttpServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:44987/");
            var checkService = new CheckService();
            IProductController productController = new ProductController();
            IShowcaseController showcaseController = new ShowcaseController(productController);
            IProductArchiveController productArchiveController = new ProductArchiveController(showcaseController, checkService);
            IPathController productPathController = new ProductPathController(productController);
            IPathController showcasetPathController = new ShowcasePathController(showcaseController);
            IPathController productOnShowcasePathController = new ProductOnShowcasePathController();
            IPathController productArchivePathController = new ProductArchivePathController(productArchiveController);
            IHttpController productHttpController = new ProductHttpController(productController, productPathController);
            IHttpController showcaseHttpController = new ShowcaseHttpController(showcasetPathController, productOnShowcasePathController, showcaseController);
            IHttpController productArchiveHttpController = new ProductArchiveHttpController(productArchiveController, productArchivePathController);
            var shopServerApplication = new ShopServerApplication(httpListener, productHttpController, showcaseHttpController, productArchiveHttpController);
            shopServerApplication.Run();
        }
    }
}
