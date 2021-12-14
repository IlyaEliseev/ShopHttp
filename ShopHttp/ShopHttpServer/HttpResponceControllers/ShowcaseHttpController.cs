using Newtonsoft.Json;
using ShopHttp.ShopHttpServer.Controllers;
using ShopHttp.ShopHttpServer.Models;
using ShopHttp.ShopModels.Models;
using System;
using System.Linq;
using System.Net;

namespace ShopHttp.ShopHttpServer.HttpResponceControllers
{
    public class ShowcaseHttpController : IHttpController
    {
        public ShowcaseHttpController(IPathController showcasePathController, IPathController productObShowcasePathController, IShowcaseController showcaseController)
        {
            ShowcasePathController = showcasePathController;
            ProductOnShowcasePathController = productObShowcasePathController;
            ShowcaseController = showcaseController;
        }

        public IShowcaseController ShowcaseController { get; set; }
        public IPathController ShowcasePathController { get; set; }
        public IPathController ProductOnShowcasePathController { get; set; }

        public void StartController(HttpListenerContext context, string path)
        {
            if (path == ShowcasePathController.Path)
            {
                switch (context.Request.HttpMethod)
                {
                    case "GET":
                        GetShowcaseInfotmation(context);
                        break;
                    case "POST":
                        CreateShowcase(context);
                        break;
                    case "PUT":
                        EditeShowcsae(context);
                        break;
                    case "PATCH":
                        PlaceProductOnShowcase(context);
                        break;
                }
            }

            if (path == ShowcasePathController.FindPath(path) && context.Request.HttpMethod == "DELETE")
            {
                DeleteShowcase(context);
            }

            if (path == ProductOnShowcasePathController.Path && context.Request.HttpMethod == "PUT")
            {
                EditeProductOnShowcase(context);
            }

            if (path == ProductOnShowcasePathController.FindPath(path) && context.Request.HttpMethod == "DELETE")
            {
                DeleteProductOnShowcase(context);
            }
        }

        private void GetShowcaseInfotmation(HttpListenerContext context)
        {
            var showcases = ShowcaseController.GetShowcases();
            var responceBody = JsonConvert.SerializeObject(showcases, Formatting.Indented);
            StreamDataController.SetResponce(responceBody, context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
        }

        private void CreateShowcase(HttpListenerContext context)
        {
            var showcasePostData = StreamDataController.GetRequestDataBody<Showcase>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var showcaseName = showcasePostData.Name;
            var showcaseVolume = showcasePostData.Volume;
            ShowcaseController.CreateShowcase(showcaseName, showcaseVolume);
            ShowcasePathController.AddPath(ShowcasePathController.Path + $"/{ShowcaseController.GetShowcaseCount()}");
            StreamDataController.SetResponce("Showcase is create", context);
            Console.WriteLine(showcasePostData);
        }

        private void EditeShowcsae(HttpListenerContext context)
        {
            var showcasePutData = StreamDataController.GetRequestDataBody<Showcase>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var showcaseId = showcasePutData.Id;
            var showcaseName = showcasePutData.Name;
            var showcaseVolume = showcasePutData.Volume;
            ShowcaseController.EditeShowcase(showcaseId, showcaseName, showcaseVolume);
            StreamDataController.SetResponce("Showcase is edit", context);
            Console.WriteLine(showcasePutData);
        }

        private void DeleteShowcase(HttpListenerContext context)
        {
            var showcaseId = int.Parse(context.Request.Url.Segments.Last());
            ShowcaseController.DeleteShowcase(showcaseId);
            StreamDataController.SetResponce("Product is delete", context);
        }

        private void PlaceProductOnShowcase(HttpListenerContext context)
        {
            var showcasePatchData = StreamDataController.GetRequestDataBody<HttpResponceModel>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var showcaseId = showcasePatchData.ShowcaseId;
            var productId = showcasePatchData.ProductId;
            ShowcaseController.PlaceProductOnShowcase(productId, showcaseId);
            ProductOnShowcasePathController.AddPath(ShowcasePathController.Path + $"/{showcaseId}" + $"/product/{ShowcaseController.GetProductCountOnShowcase(showcaseId)}");
            StreamDataController.SetResponce("Product place on showcase", context);
            Console.WriteLine(showcasePatchData);
        }

        private void EditeProductOnShowcase(HttpListenerContext context)
        {
            var productOnShowcasePutData = StreamDataController.GetRequestDataBody<HttpResponceModel>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var showcaseId = productOnShowcasePutData.ShowcaseId;
            var productId = productOnShowcasePutData.ProductInShowcaseId;
            var productName = productOnShowcasePutData.ProductName;
            var productVolume = productOnShowcasePutData.ProductVolume;
            ShowcaseController.EditeProductOnShowcase(productId, showcaseId, productName, productVolume);
            StreamDataController.SetResponce("Product on showcase is edit", context);
            Console.WriteLine(productOnShowcasePutData);
        }

        private void DeleteProductOnShowcase(HttpListenerContext context)
        {
            string stringPAth = context.Request.Url.Segments[3];
            var showcaseId = int.Parse(stringPAth.TrimEnd('/'));
            var productId = int.Parse(context.Request.Url.Segments.Last());
            ShowcaseController.DeleteProductOnShowcase(showcaseId, productId);
            StreamDataController.SetResponce("Product is delete", context);
        }
    }
}
