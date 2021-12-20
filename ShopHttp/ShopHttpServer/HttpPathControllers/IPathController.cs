namespace ShopHttp.ShopHttpServer.HttpPathControllers
{
    public interface IPathController
    {
        string Path { get; }
        void AddPath(string path);
        string FindPath(string path);
    }
}
