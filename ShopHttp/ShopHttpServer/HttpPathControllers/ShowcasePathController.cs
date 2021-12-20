using ShopHttp.ShopHttpServer.Controllers;
using System.Collections.Generic;

namespace ShopHttp.ShopHttpServer.HttpPathControllers
{
    public class ShowcasePathController : IPathController
    {
        public ShowcasePathController(IShowcaseController showcaseController)
        {
            ShowcaseController = showcaseController;
            _pathes = new List<string>();
        }

        public IShowcaseController ShowcaseController { get; }
        private List<string> _pathes;

        public string Path => "/app/showcase";

        public void AddPath(string path)
        {
            if (!_pathes.Contains(path) && _pathes.Count <= ShowcaseController.GetShowcaseCount())
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
