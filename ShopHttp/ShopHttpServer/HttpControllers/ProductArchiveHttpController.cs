using Newtonsoft.Json;
using ShopHttp.ShopHttpServer.Controllers;
using ShopHttp.ShopHttpServer.HttpPathControllers;
using ShopHttp.ShopModels.Models;
using System;
using System.Linq;
using System.Net;

namespace ShopHttp.ShopHttpServer.HttpControllers
{
    internal class ProductArchiveHttpController : IHttpController
    {
        public ProductArchiveHttpController(IProductArchiveController productArchiveController, IPathController productArchivePathController, IShowcaseController showcaseController)
        {
            ProductArchiveController = productArchiveController;
            ProductArchivePathController = productArchivePathController;
            ShowcaseController = showcaseController;
        }

        public IProductArchiveController ProductArchiveController { get; set; }
        public IShowcaseController ShowcaseController { get; set; }
        public IPathController ProductArchivePathController { get; set; }

        public void StartController(HttpListenerContext context, string path)
        {
            if (path == ProductArchivePathController.Path)
            {
                switch (context.Request.HttpMethod)
                {
                    case "GET":
                        GetArchiveInformation(context);
                        break;
                    case "POST":
                        ArchivateProduct(context);
                        break;
                    case "PATCH":
                        UnArchivateProduct(context);
                        break;
                    case "DELETE":
                        DeleteArchiveProduct(context, path);
                        break;
                }
            }
        }

        private void ArchivateProduct(HttpListenerContext context)
        {
            var archivePostData = StreamDataController.GetRequestDataBody<HttpModel>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var productId = archivePostData.ProductInShowcaseId;
            var showcaseId = archivePostData.ShowcaseId;
            if (ShowcaseController.CheckShowcaseAvailability() && ShowcaseController.GetShowcaseCount() >= showcaseId
                && ShowcaseController.GetProductCountOnShowcase(showcaseId) >= productId)
            {
                ProductArchiveController.ArchivateProduct(productId, showcaseId);
                ProductArchivePathController.AddPath(ProductArchivePathController.Path + $"/{ProductArchiveController.GetArchiveProductCount()}");
                StreamDataController.SetResponce("Product archivate", context);
                Console.WriteLine(archivePostData);
            }
            else
            {
                StreamDataController.SetResponce("Id not found", context);
            }
        }

        private void UnArchivateProduct(HttpListenerContext context)
        {
            var archivePatchData = StreamDataController.GetRequestDataBody<HttpModel>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var productId = archivePatchData.ProductInArchiveId;
            if (ProductArchiveController.CheckArchiveAvailability() && ProductArchiveController.GetArchiveProductCount() >= productId)
            {
                ProductArchiveController.UnArchivateProduct(productId);
                StreamDataController.SetResponce("Product unarchivate", context);
                Console.WriteLine(archivePatchData);
            }
            else
            {
                StreamDataController.SetResponce("Id not found", context);
            }
        }

        private void DeleteArchiveProduct(HttpListenerContext context, string path)
        {
            if (path == ProductArchivePathController.FindPath(path))
            {
                var productId = int.Parse(context.Request.Url.Segments.Last());
                if (ProductArchiveController.CheckArchiveAvailability() && ProductArchiveController.GetArchiveProductCount() >= productId)
                {
                    ProductArchiveController.DeleteArchiveProduct(productId);
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

        private void GetArchiveInformation(HttpListenerContext context)
        {
            if (ProductArchiveController.GetArchiveProductCount() > 0)
            {
                var archiveProducts = ProductArchiveController.GetArchiveProducts();
                var responceBody = JsonConvert.SerializeObject(archiveProducts, Formatting.Indented);
                StreamDataController.SetResponce(responceBody, context);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }
        }
    }
}
