using System;
using System.Net.Http;
using ShopHttp.ShopModels.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using ShopHttp.ShopHttpServer.Models;
using System.Net;

namespace ShopHttp.ShopHttpClient.Controllers
{
    public class ShowcaseHttpRequestController : IShowcaseHttpRequestController
    {
        public ShowcaseHttpRequestController(HttpClient httpClient, Uri baseUrl)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseUrl;
        }

        private readonly HttpClient _httpClient;
        private readonly string _showcasePath = "app/showcase"; 
        private readonly string _productOnShowcasePath = "app/showcase/product";

        public void CreateShowcase(string nameShowcase, double volumeShowcase)
        {
            var requestModel = new Showcase()
            {
                Name = nameShowcase,
                Volume = volumeShowcase
            };
            var jsonContent = JsonConvert.SerializeObject(requestModel);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync(_showcasePath, stringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }

        public void DeleteProductOnShowcase(int showcaseId, int productId)
        {
            var response = _httpClient.DeleteAsync(_showcasePath + $"/{showcaseId}/product/{productId}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }

        public void DeleteShowcase(int showcaseId)
        {
            var response = _httpClient.DeleteAsync(_showcasePath + $"/{showcaseId}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }

        public void EditeProductOnShowcase(int productId, int showcaseId, string productName, double productVolume)
        {
            var requestModel = new HttpModel()
            {
                ProductInShowcaseId = productId,
                ShowcaseId = showcaseId,
                ProductName = productName,
                ProductVolume = productVolume
            };
            var jsonContent = JsonConvert.SerializeObject(requestModel);
            var stringContent = new StringContent(jsonContent);
            var response = _httpClient.PutAsync(_productOnShowcasePath, stringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }

        public void EditeShowcase(int showcaseId, string showcaseName, double showcaseVolume)
        {
            var requestModel = new Showcase()
            {
                Id = showcaseId,
                Name = showcaseName,
                Volume = showcaseVolume
            };
            var jsonContent = JsonConvert.SerializeObject(requestModel);
            var stringContent = new StringContent(jsonContent);
            var response = _httpClient.PutAsync(_showcasePath, stringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }

        public void GetShowcaseInformation()
        {
            var response = _httpClient.GetAsync(_showcasePath).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var showcases = JsonConvert.DeserializeObject<List<Showcase>>(content);
                Console.WriteLine("Showcases:");
                foreach (var showcase in showcases)
                {
                    Console.WriteLine($"Id: {showcase.Id} | Name: {showcase.Name} | Volume: {showcase.Volume} | Time to Create: {showcase.TimeToCreate} | Count Products: {showcase.UnitOfWork.ProductOnShowcaseRepository.GetCount()}| VolumeCount: {showcase.VolumeCount}");
                    foreach (var p in showcase.UnitOfWork.ProductOnShowcaseRepository.GetAll())
                    {
                        Console.WriteLine($"    Id: {p.IdInShowcase} | Name: {p.Name} | Volume: {p.Volume} | Time to Create: {p.TimeToCreate}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Showcases empty");
            }
        }

        public void PlaceProductOnShowcase(int productId, int showcaseId)
        {
            var requestModel = new HttpModel()
            {
                ProductId = productId,
                ShowcaseId = showcaseId
            };
            var jsonContent = JsonConvert.SerializeObject(requestModel);
            var stringContent = new StringContent(jsonContent);
            var response = _httpClient.PatchAsync(_showcasePath, stringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }
    }
}
