using Newtonsoft.Json;
using ShopHttp.ShopHttpServer.Controllers;
using ShopHttp.ShopHttpServer.Services;
using ShopHttp.ShopModels.Models;
using System;
using System.Linq;
using System.Net;

namespace ShopHttp.ShopHttpServer.HttpResponceControllers
{
    internal class ProductArchiveHttpController : IHttpController
    {
        public ProductArchiveHttpController(IProductArchiveController productArchiveController, IPathController productArchivePathController)
        {
            ProductArchiveController = productArchiveController;
            ProductArchivePathController = productArchivePathController;
        }

        public IProductArchiveController ProductArchiveController { get; set; }
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
                        try
                        {
                            ArchivateProduct(context);
                        }
                        catch (IdNotFoundException ex)
                        {
                            StreamDataController.SetResponce(ex.Message, context);
                        }
                        break;
                    case "PATCH":
                        try
                        {
                            UnArchivateProduct(context);
                        }
                        catch (IdNotFoundException ex)
                        {
                            StreamDataController.SetResponce(ex.Message, context);
                        }
                        break;
                }
            }

            if (path == ProductArchivePathController.FindPath(path) && context.Request.HttpMethod == "DELETE")
            {
                DeleteArchiveProduct(context);
            }
        }


        private void ArchivateProduct(HttpListenerContext context)
        {
            var archivePostData = StreamDataController.GetRequestDataBody<HttpResponceModel>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var productId = archivePostData.ProductInShowcaseId;
            var showcaseId = archivePostData.ShowcaseId;
            ProductArchiveController.ArchivateProduct(productId, showcaseId);
            ProductArchivePathController.AddPath(ProductArchivePathController.Path + $"/{ProductArchiveController.GetArchiveProductCount()}");
            StreamDataController.SetResponce("Product is archivate", context);
            Console.WriteLine(archivePostData);
        }

        private void UnArchivateProduct(HttpListenerContext context)
        {
            var archivePatchData = StreamDataController.GetRequestDataBody<HttpResponceModel>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var productId = archivePatchData.ProductInArchiveId;
            ProductArchiveController.UnArchivateProduct(productId);
            StreamDataController.SetResponce("Product is unarchivate", context);
            Console.WriteLine(archivePatchData);
        }

        private void DeleteArchiveProduct(HttpListenerContext context)
        {
            var productId = int.Parse(context.Request.Url.Segments.Last());
            ProductArchiveController.DeleteArchiveProduct(productId);
            StreamDataController.SetResponce("Product is delete", context);
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
