using ShopHttp.ShopHttpServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShopHttp.ShopHttpServer.DAL
{
    public class ProductOnShowcaseRepository : IProductOnShowcaseRepository
    {
        public ProductOnShowcaseRepository(List<Product> productOnShowcaseContext)
        {
            Context = productOnShowcaseContext;
        }

        public List<Product> Context { get; private set; }

        public void Add(Product entity)
        {
            Context.Add(entity);
        }

        public void DeleteById(int id)
        {
            Context.RemoveAll(x => x.IdInShowcase == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return Context.ToList();
        }

        public Product GetById(int id)
        {
            return Context.SingleOrDefault(x => x.IdInShowcase == id);
        }

        public int GetCount()
        {
            return Context.Count;
        }
    }
}
