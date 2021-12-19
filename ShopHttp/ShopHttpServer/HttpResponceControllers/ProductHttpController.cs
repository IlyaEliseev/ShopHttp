using Newtonsoft.Json;
using ShopHttp.ShopHttpServer.Controllers;
using ShopHttp.ShopModels.Models;
using System;
using System.Linq;
using System.Net;

namespace ShopHttp.ShopHttpServer.HttpResponceControllers
{
    public class ProductHttpController : IHttpController
    {
        public ProductHttpController(IProductController productController, IPathController productPathController)
        {
            ProductController = productController;
            ProductPathController = productPathController;
        }

        public IProductController ProductController { get; set; }
        public IPathController ProductPathController { get; set; }

        public void StartController(HttpListenerContext context, string path)
        {
            if (path == ProductPathController.Path)
            {
                switch (context.Request.HttpMethod)
                {
                    case "GET":
                        GetProductInformation(context);
                        break;
                    case "POST":
                        CreateProduct(context);
                        break;
                    case "PUT":
                        EditProduct(context);
                        break;
                    case "DELETE":
                        DeleteProduct(context, path);
                        break;
                }
            }
        }

        private void GetProductInformation(HttpListenerContext context)
        {
            if (ProductController.GetProductCount() > 0)
            {
                var products = ProductController.GetProducts();
                var responceBody = JsonConvert.SerializeObject(products, Formatting.Indented);
                StreamDataController.SetResponce(responceBody, context);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }    
        }

        private void CreateProduct(HttpListenerContext context)
        {
            var productPostData = StreamDataController.GetRequestDataBody<ProductModel>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var productName = productPostData.Name;
            var productVolume = productPostData.Volume;
            ProductController.CreateProduct(productName, productVolume);
            ProductPathController.AddPath(ProductPathController.Path + $"/{ProductController.GetProductCount()}");
            StreamDataController.SetResponce("Product create", context);
            Console.WriteLine(productPostData);
        }

        private void EditProduct(HttpListenerContext context) 
        {
            var productPutData = StreamDataController.GetRequestDataBody<ProductModel>(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var productId = productPutData.IdInProductList;
            var productName = productPutData.Name;
            var productVolume = productPutData.Volume;
            if (ProductController.CheckProductAvailability() && ProductController.GetProductCount() >= productId)
            {
                ProductController.EditProduct(productId, productName, productVolume);
                StreamDataController.SetResponce("Product edit", context);
                Console.WriteLine(productPutData);
            }
            else
            {
                StreamDataController.SetResponce("Id not found", context);
            }
        }

        private void DeleteProduct(HttpListenerContext context, string path) 
        {
            if (path == ProductPathController.FindPath(path))
            {
                var productId = int.Parse(context.Request.Url.Segments.Last());
                if (ProductController.CheckProductAvailability() && ProductController.GetProductCount() > productId)
                {
                    ProductController.DeleteProduct(productId);
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
