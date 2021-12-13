namespace ShopHttp.ShopHttpClient.Controllers
{
    public interface IProductArchiveHttpRequestController
    {
        void ArchivateProduct(int productId, int showcaseId);
        void GetArchiveInformation();
        void UnArchivateProduct(int productId);
        void DeleteArchiveProduct(int productId);

    }
}
