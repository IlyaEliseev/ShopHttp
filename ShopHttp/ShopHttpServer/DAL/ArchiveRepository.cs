using ShopHttp.ShopHttpServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShopHttp.ShopHttpServer.DAL
{
    public class ArchiveRepository : IArchiveRepository
    {
        public ArchiveRepository(List<Product> archiveProductContext)
        {
            Context = archiveProductContext;
        }

        public List<Product> Context { get; private set; }

        public IEnumerable<Product> GetAll()
        {
            return Context.ToList();
        }

        public Product GetById(int id)
        {
            return Context.SingleOrDefault(x => x.IdInArchive == id);
        }

        public void Add(Product entity)
        {
            Context.Add(entity);
        }

        public void DeleteById(int id)
        {
            Context.RemoveAll(x => x.IdInArchive == id);
        }

        public int GetCount()
        {
            return Context.Count;
        }
    }
}