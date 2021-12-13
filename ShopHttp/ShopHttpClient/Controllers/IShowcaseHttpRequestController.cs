namespace ShopHttp.ShopHttpClient.Controllers
{
    public interface IShowcaseHttpRequestController
    {
        void CreateShowcase(string nameShowcase, double volumeShowcase);
        void DeleteShowcase(int showcaseId);
        void PlaceProductOnShowcase(int productId, int showcaseId);
        void DeleteProductOnShowcase(int showcaseId, int productId);
        void EditeShowcase(int showcaseId, string showcaseName, double showcaseVolume);
        void EditeProductOnShowcase(int productId, int showcaseId, string productName, double productVolume);
        void GetShowcaseInformation();
    }
}
