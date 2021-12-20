using ShopHttp.ShopHttpServer.Controllers;
using ShopHttp.ShopHttpServer.HttpPathControllers;
using ShopHttp.ShopHttpServer.HttpControllers;
using System.Net;

namespace ShopHttp.ShopHttpServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:44987/");
            IProductController productController = new ProductController();
            IShowcaseController showcaseController = new ShowcaseController(productController);
            IProductArchiveController productArchiveController = new ProductArchiveController(showcaseController);
            IPathController productPathController = new ProductPathController(productController);
            IPathController showcasetPathController = new ShowcasePathController(showcaseController);
            IPathController productOnShowcasePathController = new ProductOnShowcasePathController();
            IPathController productArchivePathController = new ProductArchivePathController(productArchiveController);
            IHttpController productHttpController = new ProductHttpController(productController, productPathController);
            IHttpController showcaseHttpController = new ShowcaseHttpController(showcasetPathController, productOnShowcasePathController, productController, showcaseController);
            IHttpController productArchiveHttpController = new ProductArchiveHttpController(productArchiveController, productArchivePathController, showcaseController);
            var shopServerApplication = new ShopServerApplication(httpListener, productHttpController, showcaseHttpController, productArchiveHttpController);
            shopServerApplication.Run();
        }
    }
}
