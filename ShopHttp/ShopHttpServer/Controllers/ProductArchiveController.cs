using ShopHttp.ShopHttpServer.DAL;
using ShopHttp.ShopHttpServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopHttp.ShopHttpServer.Controllers
{
    public class ProductArchiveController : IProductArchiveController
    {
        public ProductArchiveController(IShowcaseController showcaseController)
        {
            ShowcaseController = showcaseController;
            UnitOfWork = new UnitOfWork();
        }
        
        public IShowcaseController ShowcaseController { get; }
        public IUnitOfWork UnitOfWork { get; }

        public void ArchivateProduct(int productId, int showcaseId)
        {
            var selectShowcase = ShowcaseController.GetShowcaseById(showcaseId);
            var selectProduct = selectShowcase.GetProduct(productId);
            UnitOfWork.ArchiveRepository.Add(selectProduct);
            selectProduct.IdInArchive = GetArchiveProductCount();
            selectProduct.TimeToArchive = DateTime.Now;
            ShowcaseController.DeleteProductOnShowcase(showcaseId, productId);
        }

        public bool CheckArchiveAvailability()
        {
            if (GetArchiveProductCount() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void DeleteArchiveProduct(int productId)
        {
            UnitOfWork.ArchiveRepository.DeleteById(productId);
            var products = from p in UnitOfWork.ArchiveRepository.GetAll()
                           select p;
            for (int i = 0; i < UnitOfWork.ArchiveRepository.GetCount(); i++)
            {
                products.ElementAtOrDefault(i).IdInArchive = i + 1;
            }
        }

        public int GetArchiveProductCount()
        {
            return UnitOfWork.ArchiveRepository.GetCount();
        }

        public void UnArchivateProduct(int productId)
        {
            var selectProduct = UnitOfWork.ArchiveRepository.GetById(productId);
            var selectShowcase = ShowcaseController.GetShowcaseById(selectProduct.IdShowcase);
            selectShowcase.UnitOfWork.ProductOnShowcaseRepository.Add(selectProduct);
            selectProduct.IdInShowcase = selectShowcase.GetProductCount();
            UnitOfWork.ArchiveRepository.DeleteById(productId);
            var products = from p in UnitOfWork.ArchiveRepository.GetAll()
                           select p;
            for (int i = 0; i < UnitOfWork.ArchiveRepository.GetCount(); i++)
            {
                products.ElementAtOrDefault(i).IdInArchive = i + 1;
            }
        }

        public IEnumerable<Product> GetArchiveProducts()
        {
            return UnitOfWork.ArchiveRepository.GetAll();
        }
    }
}
