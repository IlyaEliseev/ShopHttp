using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using ShopHttp.ShopModels.Models;

namespace ShopHttp.ShopHttpClient.Controllers
{
    public class ProductArchiveHttpRequestController : IProductArchiveHttpRequestController
    {
        public ProductArchiveHttpRequestController(HttpClient httpClient, Uri baseUrl)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseUrl;
        }

        private readonly HttpClient _httpClient;
        private readonly string _productArchivePath = "app/archiveProduct";

        public void ArchivateProduct(int productId, int showcaseId)
        {
            var requestModel = new HttpModel()
            {
                ProductInShowcaseId = productId,
                ShowcaseId = showcaseId
            };
            var jsonContent = JsonConvert.SerializeObject(requestModel);
            var stringContent = new StringContent(jsonContent);
            var response = _httpClient.PostAsync(_productArchivePath, stringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }

        public void DeleteArchiveProduct(int productId)
        {
            var response = _httpClient.DeleteAsync(_productArchivePath + $"/{productId}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }

        public void GetArchiveInformation()
        {
            var response = _httpClient.GetAsync(_productArchivePath).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var archiveProducts = JsonConvert.DeserializeObject<List<ProductModel>>(content);
                Console.WriteLine("Archive:");
                foreach (var product in archiveProducts)
                {
                    Console.WriteLine($"Id: {product.IdInArchive} | Name product: {product.Name} | Volume product: {product.Volume} | Time to create: {product.TimeToArchive}");
                }
            }
            else
            {
                Console.WriteLine("Archive empty");
            }
        }

        public void UnArchivateProduct(int productId)
        {
            var requestModel = new HttpModel()
            {
                ProductInArchiveId = productId,
            };
            var jsonContent = JsonConvert.SerializeObject(requestModel);
            var stringContent = new StringContent(jsonContent);
            var response = _httpClient.PatchAsync(_productArchivePath, stringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }
    }
}
