namespace ShopHttp.ShopHttpClient.Controllers
{
    public interface IProductHttpRequestController
    {
        void CreateProduct(string productName, double productVolume);
        void EditProduct(int productId, string productName, double productVolume);
        void DeleteProduct(int productId);
        void GetProductInformation();
    }
}