using ShopHttp.ShopHttpServer.Controllers;
using System.Collections.Generic;

namespace ShopHttp.ShopHttpServer.HttpResponceControllers
{
    public class ProductArchivePathController : IPathController
    {
        public ProductArchivePathController(IProductArchiveController productArchiveController)
        {
            ProductArchiveController = productArchiveController;
            _pathes = new List<string>();
        }

        private List<string> _pathes;
        public IProductArchiveController ProductArchiveController { get; set; }
        public string Path => "/app/archiveProduct";

        //add if collection not contains path
        public void AddPath(string path)
        {
            if (!_pathes.Contains(path) && _pathes.Count <= ProductArchiveController.GetArchiveProductCount())
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
