using System;

namespace ShopHttp.ShopHttpServer.Models
{
    public class Product 
    {
        public Product()
        {
            
        }
        
        public Product(string productName, double volume)
        {
            Name = productName;
            Volume = volume;
            TimeToCreate = DateTime.Now;
        }

        public int IdInProductList { get; set; }
        public int IdInShowcase { get; set; }
        public int IdInArchive { get; set; }
        public int IdShowcase{ get; set; }
        public string Name { get; set; }
        public double Volume { get; set; }
        public DateTime TimeToCreate { get; set; }
        public DateTime TimeToArchive { get; set; }
    }
}
