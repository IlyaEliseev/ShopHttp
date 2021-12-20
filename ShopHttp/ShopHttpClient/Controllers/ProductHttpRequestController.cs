using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ShopHttp.ShopModels.Models;
using System.Collections.Generic;
using System.Net;

namespace ShopHttp.ShopHttpClient.Controllers
{
    public class ProductHttpRequestController : IProductHttpRequestController
    {
        public ProductHttpRequestController(HttpClient httpClient, Uri baseUrl)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseUrl;
        }

        private readonly HttpClient _httpClient;
        private readonly string _productPath = "app/product";

        public void CreateProduct(string productName, double productVolume)
        {
            var requestModel = new ProductModel()
            {
                Name = productName,
                Volume = productVolume
            };
            var jsonContent = JsonConvert.SerializeObject(requestModel);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "aplication/json");
            var response = _httpClient.PostAsync(_productPath, stringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }

        public void DeleteProduct(int productId)
        {
            var response = _httpClient.DeleteAsync(_productPath + $"/{productId}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }

        public void EditProduct(int productId, string productName, double productVolume)
        {
            var requestModel = new ProductModel()
            {
                IdInProductList = productId,
                Name = productName,
                Volume = productVolume
            };
            var jsonContent = JsonConvert.SerializeObject(requestModel);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "aplication/json");
            var response = _httpClient.PutAsync(_productPath, stringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }

        public void GetProductInformation()
        {
            var response = _httpClient.GetAsync(_productPath).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var products = JsonConvert.DeserializeObject<List<ProductModel>>(content);
                Console.WriteLine("Products:");
                foreach (var product in products)
                {
                    Console.WriteLine($"Id: {product.IdInProductList} | Name product: {product.Name} | Volume product: {product.Volume} | Time to create: {product.TimeToCreate}");
                }
            }
            else
            {
                Console.WriteLine("Product list empty");
            }
        }
    }
}
