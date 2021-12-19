using ShopHttp.ShopHttpServer.Models;
using System.Collections.Generic;

namespace ShopHttp.ShopHttpServer.Controllers
{
    public interface IProductController
    {
        void CreateProduct(string nameProduct, double volumeProduct);
        void EditProduct(int productId, string nameProduct, double volumeProduct);
        void DeleteProduct(int productId);
        bool CheckProductAvailability();
        Product GetProduct(int id);
        IEnumerable<Product> GetProducts();
        int GetProductCount();
    }
}
