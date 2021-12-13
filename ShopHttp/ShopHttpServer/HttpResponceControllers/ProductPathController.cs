using ShopHttp.ShopHttpServer.Controllers;
using System.Collections.Generic;

namespace ShopHttp.ShopHttpServer.HttpResponceControllers
{
    public class ProductPathController : IPathController
    {
        public ProductPathController(IProductController productController)
        {
            ProductController = productController;
            _pathes = new List<string>();
        }

        public IProductController ProductController { get; }
        private List<string> _pathes; 

        public string Path => "/app/product";

        public void AddPath(string path)
        {
            if (!_pathes.Contains(path) && _pathes.Count <= ProductController.GetProductCount())
            {
                _pathes.Add(path);
            }
        }

        public string FindPath(string path)
        {
            return _pathes.Find(x => x == path);
        }
    }
}
