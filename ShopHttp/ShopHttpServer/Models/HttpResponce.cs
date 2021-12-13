namespace ShopHttp.ShopHttpServer.Models
{
    public class HttpResponce
    {
        public int ProductId { get; set; }
        public int ShowcaseId { get; set; }
        public int ProductInShowcaseId { get; set; }
        public int ProductInArchiveId { get; set; }
        public string ProductName { get; set; }
        public string ShowcaseName { get; set; }
        public double ProductVolume { get; set; }
        public double ShowacaseVolume { get; set; }
    }
}
