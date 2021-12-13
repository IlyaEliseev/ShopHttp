using System.Collections.Generic;

namespace ShopHttp.ShopHttpServer.DAL
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void DeleteById(int id);
        int GetCount();
    }
}
