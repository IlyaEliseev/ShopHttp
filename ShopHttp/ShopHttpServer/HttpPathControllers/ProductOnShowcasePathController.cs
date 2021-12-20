using System.Collections.Generic;

namespace ShopHttp.ShopHttpServer.HttpPathControllers
{
    public class ProductOnShowcasePathController : IPathController
    {
        public ProductOnShowcasePathController()
        {
            _pathes = new List<string>();
        }

        private List<string> _pathes;
        public string Path => "/app/showcase/product";

        public void AddPath(string path)
        {
            if (!_pathes.Contains(path))
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
