using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace ShopHttp.ShopHttpServer.Controllers
{
    public class StreamDataController
    {
        public static T GetRequestDataBody<T>(HttpListenerContext context)
        {
            string requestBody;
            using (var reader = new StreamReader(context.Request.InputStream,
                                     context.Request.ContentEncoding))
            {
                requestBody = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(requestBody);
        }

        public static void SetResponce(string responce, HttpListenerContext context)
        {
            var streamWrite = context.Response.OutputStream;
            var bytes = Encoding.UTF8.GetBytes(responce);
            streamWrite.Write(bytes, 0, bytes.Length);
            streamWrite.Close();
        }
    }
}
