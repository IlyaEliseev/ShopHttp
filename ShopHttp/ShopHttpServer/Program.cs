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
            var notifyService = new NotifyService();
            var checkService = new CheckService();
            IProductController productController = new ProductController(notifyService, checkService);
            IShowcaseController showcaseController = new ShowcaseController(notifyService, checkService, productController);
            IProductArchiveController productArchiveController = new ProductArchiveController(notifyService, showcaseController, checkService);
            IPathController productPathController = new ProductPathController(productController);
            IPathController showcasetPathController = new ShowcasePathController(showcaseController);
            IPathController productOnShowcasePathController = new ProductOnShowcasePathController();
            IPathController productArchivePathController = new ProductArchivePathController(productArchiveController);
            ProductHttpController productHttpController = new ProductHttpController(productController, productPathController);
            ShowcaseHttpController showcaseHttpController = new ShowcaseHttpController(showcasetPathController, productOnShowcasePathController, showcaseController);
            ProductArchiveHttpController productArchiveHttpController = new ProductArchiveHttpController(productArchiveController, productArchivePathController);
            var shopServerApplication = new ShopServerApplication(httpListener, productHttpController, showcaseHttpController, productArchiveHttpController);
            shopServerApplication.Run();
        }
    }
}
