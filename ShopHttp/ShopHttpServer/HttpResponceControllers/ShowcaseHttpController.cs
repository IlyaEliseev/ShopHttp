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
        public ShowcaseHttpController(IPathController showcasePathController, IPathController productObShowcasePathController, IProductController productController, IShowcaseController showcaseController)
        {
            ProductController = productController;
            ShowcasePathController = showcasePathController;
            ProductOnShowcasePathController = productObShowcasePathController;
            ShowcaseController = showcaseController;
        }

        public IProductController ProductController { get; set; }
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
                    case "DELETE":
                        DeleteShowcase(context, path);
                        break;
                }
            }

            if (path == ProductOnShowcasePathController.Path && context.Request.HttpMethod == "PUT")
            {
                EditeProductOnShowcase(context);
            }
            
            if (context.Request.HttpMethod == "DELETE")
            {
                DeleteProductOnShowcase(context, path);
            }
        }

        private void GetShowcaseInfotmation(HttpListenerContext context)
        {
            if (ShowcaseController.GetShowcaseCount() > 0)
            {
                var showcases = ShowcaseController.GetShowcases();
                var responceBody = JsonConvert.SerializeObject(showcases, Formatting.Indented);
                StreamDataController.SetResponce(responceBody, context);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }
        }

        private void CreateShowcase(HttpListenerContext context)
        {
            var showcasePostData = StreamDataController.GetRequestDataBody<Showcase>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var showcaseName = showcasePostData.Name;
            var showcaseVolume = showcasePostData.Volume;
            ShowcaseController.CreateShowcase(showcaseName, showcaseVolume);
            ShowcasePathController.AddPath(ShowcasePathController.Path + $"/{ShowcaseController.GetShowcaseCount()}");
            StreamDataController.SetResponce("Showcase create", context);
            Console.WriteLine(showcasePostData);
        }

        private void EditeShowcsae(HttpListenerContext context)
        {
            var showcasePutData = StreamDataController.GetRequestDataBody<Showcase>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var showcaseId = showcasePutData.Id;
            var showcaseName = showcasePutData.Name;
            var showcaseVolume = showcasePutData.Volume;
            if (ShowcaseController.CheckShowcaseAvailability() && ShowcaseController.GetShowcaseCount() >= showcaseId)
            {
                if (ShowcaseController.GetProductCountOnShowcase(showcaseId) == 0)
                {
                    ShowcaseController.EditeShowcase(showcaseId, showcaseName, showcaseVolume);
                    StreamDataController.SetResponce("Showcase edit", context);
                    Console.WriteLine(showcasePutData);
                }
                else
                {
                    StreamDataController.SetResponce("Showcase not empty. You can only delete empty showcases", context);
                }
            }
            else
            {
                StreamDataController.SetResponce("Id not found", context);
            }
        }

        private void DeleteShowcase(HttpListenerContext context, string path)
        {
            if (path == ShowcasePathController.FindPath(path))
            {
                var showcaseId = int.Parse(context.Request.Url.Segments.Last());
                if (ShowcaseController.CheckShowcaseAvailability() && ShowcaseController.GetShowcaseCount() >= showcaseId)
                {
                    if (ShowcaseController.CheckShowcaseCount(showcaseId))
                    {
                        ShowcaseController.DeleteShowcase(showcaseId);
                        StreamDataController.SetResponce("Showcase delete", context);
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        StreamDataController.SetResponce("Showcase not empty. You can only delete empty showcases", context);
                    }
                }
                else
                {
                    StreamDataController.SetResponce("Id not found", context);
                }
            }
            else
            {
                StreamDataController.SetResponce("Id not found", context);
            }
        }

        private void PlaceProductOnShowcase(HttpListenerContext context)
        {
            var showcasePatchData = StreamDataController.GetRequestDataBody<HttpResponceModel>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var showcaseId = showcasePatchData.ShowcaseId;
            var productId = showcasePatchData.ProductId;
            if (ShowcaseController.GetShowcaseCount() >= showcaseId && ProductController.GetProductCount() >= productId)
            {
                if (ShowcaseController.CheckShowcaseVolumeOverflow(showcaseId, productId))
                {
                    ShowcaseController.PlaceProductOnShowcase(productId, showcaseId);
                    ProductOnShowcasePathController.AddPath(ShowcasePathController.Path + $"/{showcaseId}" + $"/product/{ShowcaseController.GetProductCountOnShowcase(showcaseId)}");
                    StreamDataController.SetResponce("Product place on showcase", context);
                    Console.WriteLine(showcasePatchData);
                }
                else
                {
                    StreamDataController.SetResponce("Not enough space on showcase", context);
                }
            }
            else
            {
                StreamDataController.SetResponce("Id not found", context);
            }
        }

        private void EditeProductOnShowcase(HttpListenerContext context)
        {
            var productOnShowcasePutData = StreamDataController.GetRequestDataBody<HttpResponceModel>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var showcaseId = productOnShowcasePutData.ShowcaseId;
            var productId = productOnShowcasePutData.ProductInShowcaseId;
            var productName = productOnShowcasePutData.ProductName;
            var productVolume = productOnShowcasePutData.ProductVolume;
            if (ShowcaseController.CheckShowcaseAvailability() && ShowcaseController.GetShowcaseCount() >= showcaseId)
            {
                if (ShowcaseController.CheckProductOnCurrentShowcase(showcaseId))
                {
                    if (productVolume <= ShowcaseController.GetShowcaseFreeSpace(showcaseId))
                    {
                        ShowcaseController.EditeProductOnShowcase(productId, showcaseId, productName, productVolume);
                        StreamDataController.SetResponce("Product on showcase edit", context);
                        Console.WriteLine(productOnShowcasePutData);
                    }
                    else
                    {
                        StreamDataController.SetResponce("Not enough space on showcase", context);
                    }
                }
                else
                {
                    StreamDataController.SetResponce("That product not found", context);
                }
            }
            else
            {
                StreamDataController.SetResponce("Id not found", context);
            }
        }

        private void DeleteProductOnShowcase(HttpListenerContext context, string path)
        {
            if (path == ProductOnShowcasePathController.FindPath(path))
            {
                string stringPAth = context.Request.Url.Segments[3];
                var showcaseId = int.Parse(stringPAth.TrimEnd('/'));
                var productId = int.Parse(context.Request.Url.Segments.Last());
                if (ShowcaseController.CheckShowcaseAvailability() && ShowcaseController.GetShowcaseCount() >= showcaseId &&
                                                                    ShowcaseController.CheckProductOnCurrentShowcase(showcaseId))
                {

                    ShowcaseController.DeleteProductOnShowcase(showcaseId, productId);
                    StreamDataController.SetResponce("Product delete", context);
                }
                else
                {
                    StreamDataController.SetResponce("Id not found", context);
                }
            }
            else
            {
                StreamDataController.SetResponce("Id not found", context);
            }
        }
    }
}
