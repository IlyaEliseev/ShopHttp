using System;
using System.IO;
using System.Net;
using ShopHttp.ShopHttpServer.HttpResponceControllers;

namespace ShopHttp.ShopHttpServer
{
    internal class ShopServerApplication
    {
        public ShopServerApplication(HttpListener httpListener, IHttpController productHttpController, IHttpController showcasetHttpController,
                                    IHttpController productArchiveHttpController)
        {
            _httpListener = httpListener;
            ProductHttpController = productHttpController;
            ShowcasetHttpController = showcasetHttpController;
            ProductArchiveHttpController = productArchiveHttpController;
        }

        private readonly HttpListener _httpListener;
        public IHttpController ProductHttpController { get; }
        public IHttpController ShowcasetHttpController { get; }
        public IHttpController ProductArchiveHttpController { get; }

        internal void Run()
        {
            _httpListener.Start();
            Console.WriteLine("Server started");
            try
            {
                while (true)
                {
                    var context = _httpListener.GetContext();
                    var path = context.Request.Url.PathAndQuery;
                    ProductHttpController.StartController(context, path);
                    ShowcasetHttpController.StartController(context, path);
                    ProductArchiveHttpController.StartController(context, path);
                    context.Response.Close();
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("serverLogs.log", ex.ToString());
            }
            finally
            {
                _httpListener.Stop();
            }
        }
    }
}
