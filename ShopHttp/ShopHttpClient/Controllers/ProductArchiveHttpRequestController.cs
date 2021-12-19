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
            var newResponce = new HttpResponceModel()
            {
                ProductInShowcaseId = productId,
                ShowcaseId = showcaseId
            };
            var jsonResponce = JsonConvert.SerializeObject(newResponce);
            var stringResponce = new StringContent(jsonResponce);
            var responce = _httpClient.PostAsync(_productArchivePath, stringResponce).Result;
            var content = responce.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }

        public void DeleteArchiveProduct(int productId)
        {
            var responce = _httpClient.DeleteAsync(_productArchivePath + $"/{productId}").Result;
            if ((int)responce.StatusCode == 200)
            {
                var content = responce.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine("Id not found");
            }
        }

        public void GetArchiveInformation()
        {
            var responce = _httpClient.GetAsync(_productArchivePath).Result;
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                var content = responce.Content.ReadAsStringAsync().Result;
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
            var newResponce = new HttpResponceModel()
            {
                ProductInArchiveId = productId,
            };
            var jsonResponce = JsonConvert.SerializeObject(newResponce);
            var stringResponce = new StringContent(jsonResponce);
            var responce = _httpClient.PatchAsync(_productArchivePath, stringResponce).Result;
            var content = responce.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }
    }
}
