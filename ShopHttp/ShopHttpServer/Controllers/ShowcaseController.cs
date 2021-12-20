using ShopHttp.ShopHttpServer.DAL;
using ShopHttp.ShopHttpServer.Models;
using System.Linq;
using System.Collections.Generic;

namespace ShopHttp.ShopHttpServer.Controllers 
{
    public class ShowcaseController : IShowcaseController
    {
        public ShowcaseController(IProductController productController)
        {
            ProductController = productController;
            UnitOfWork = new UnitOfWork();
        }

        public IUnitOfWork UnitOfWork { get; }
        public IProductController ProductController { get; }

        public void CreateShowcase(string showcaseName, double showcaseVolume)
        {
            Showcase showcase = new Showcase(showcaseName, showcaseVolume);
            UnitOfWork.ShowcaseRepository.Add(showcase);
            showcase.Id = UnitOfWork.ShowcaseRepository.GetCount();
        }

        public void DeleteShowcase(int showcaseId)
        {
            UnitOfWork.ShowcaseRepository.DeleteById(showcaseId);
            var showcase = from s in UnitOfWork.ShowcaseRepository.GetAll()
                           select s;
            for (int i = 0; i < GetShowcaseCount(); i++)
            {
                showcase.ElementAtOrDefault(i).Id = i + 1;
            }
        }

        public void PlaceProductOnShowcase(int productId, int showcaseId)
        {
            var selectProduct = ProductController.GetProduct(productId);
            var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId);
            selectShowcase.UnitOfWork.ProductOnShowcaseRepository.Add(selectProduct);
            SumShowcaseVolume(showcaseId, productId);
            ProductController.DeleteProduct(productId);
            selectProduct.IdInShowcase = selectShowcase.GetProductCount();
            selectProduct.IdShowcase = showcaseId;
        }

        public void DeleteProductOnShowcase(int showcaseId, int productId)
        {
            var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId);
            var selectProduct = selectShowcase.GetProduct(productId);
            selectShowcase.UnitOfWork.ProductOnShowcaseRepository.DeleteById(productId);
            selectShowcase.VolumeCount -= selectProduct.Volume;
            var products = from p in selectShowcase.UnitOfWork.ProductOnShowcaseRepository.GetAll()
                           select p;
            for (int i = 0; i < products.Count(); i++)
            {
                products.ElementAtOrDefault(i).IdInShowcase = i + 1;
            }
        }

        public void EditeShowcase(int showcaseId, string showcaseName, double showcaseVolume)
        {
            var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId); 
            var products = from p in selectShowcase.UnitOfWork.ProductOnShowcaseRepository.GetAll()
                           select p;
            selectShowcase.Name = showcaseName;
            selectShowcase.Volume = showcaseVolume; 
        }

        public void EditeProductOnShowcase(int productId, int showcaseId, string productName, double productVolume)
        {
            var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId);
            var selectProduct = selectShowcase.UnitOfWork.ProductOnShowcaseRepository.GetById(productId);
            selectProduct.Name = productName;
            selectProduct.Volume = productVolume;
        }

        public bool CheckShowcaseAvailability()
        {
            if (GetShowcaseCount() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public double GetShowcaseFreeSpace(int showcaseId)
        {
            var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId);
            double freespace = selectShowcase.Volume - selectShowcase.VolumeCount;
            return freespace;
        }

        public int GetShowcaseCount()
        {
            return UnitOfWork.ShowcaseRepository.GetCount();
        }

        public void SumShowcaseVolume(int showcaseId, int productId)
        {
            var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId);
            var selectProduct = ProductController.GetProduct(productId);
            selectShowcase.VolumeCount += selectProduct.Volume;
        }

        public bool CheckShowcaseCount(int showcaseId)
        {
            var findShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId);
            if (findShowcase.UnitOfWork.ProductOnShowcaseRepository.GetCount() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckShowcaseVolumeOverflow(int showcaseId, int productId)
        {
            var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId);
            var selectProduct = ProductController.GetProduct(productId);
            if (selectShowcase.VolumeCount <= selectShowcase.Volume && GetShowcaseFreeSpace(showcaseId) >= selectProduct.Volume)
            {
                return true;
            }
            return false;
        }

        public bool CheckProductOnCurrentShowcase(int showcaseId)
        {
            var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId); 
            if (selectShowcase.UnitOfWork.ProductOnShowcaseRepository.GetCount() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Showcase GetShowcaseById(int showcaseId)
        {
            return UnitOfWork.ShowcaseRepository.GetById(showcaseId);
        }

        public int GetProductCountOnShowcase(int showcaseId)
        {
            var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId);
            int count = selectShowcase.UnitOfWork.ProductOnShowcaseRepository.GetCount();
            return count;
        }

        public IEnumerable<Showcase> GetShowcases()
        {
            return UnitOfWork.ShowcaseRepository.GetAll();
        }
    }
}
