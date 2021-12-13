using ShopHttp.ShopHttpServer.Models;

namespace ShopHttp.ShopHttpServer.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private ShopContext _context = new ShopContext();
        private IProductRepository _productRepository;
        private IShowcaseRepository _showcaseRepository;
        private IProductOnShowcaseRepository _productOnShowcaseRepository;
        private IArchiveRepository _archiveRepository;

        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository==null)
                {
                    _productRepository = new ProductRepository(_context.ProductContext);
                }
                return _productRepository;
            }
        }

        public IShowcaseRepository ShowcaseRepository
        {
            get
            {
                if (_showcaseRepository == null)
                {
                    _showcaseRepository = new ShowcaseRepository(_context.ShowcaseContext);
                }
                return _showcaseRepository;
            }
        }

        public IProductOnShowcaseRepository ProductOnShowcaseRepository
        {
            get
            {
                if (_productOnShowcaseRepository == null)
                {
                    _productOnShowcaseRepository = new ProductOnShowcaseRepository(_context.ProductOnShowcaseContext);
                }
                return _productOnShowcaseRepository;
            }
        }
        public IArchiveRepository ArchiveRepository
        {
            get
            {
                if (_archiveRepository == null)
                {
                    _archiveRepository = new ArchiveRepository(_context.ArchiveContext); 
                }
                return _archiveRepository;
            }
        }
    }
}
