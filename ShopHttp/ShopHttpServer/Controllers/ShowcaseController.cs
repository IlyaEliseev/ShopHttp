using ShopHttp.ShopHttpServer.DAL;
using ShopHttp.ShopHttpServer.Models;
using ShopHttp.ShopHttpServer.Services;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ShopHttp.ShopHttpServer.Controllers 
{
    public class ShowcaseController : IShowcaseController
    {

        public ShowcaseController(NotifyService notifyService, CheckService checkService, IProductController productController)
        {
            NotifyService = notifyService;
            CheckService = checkService;
            ProductController = productController;
            UnitOfWork = new UnitOfWork();
        }

        public IUnitOfWork UnitOfWork { get; }
        public IProductController ProductController { get; }
        public NotifyService NotifyService { get; }
        public CheckService CheckService { get; }

        public void CreateShowcase(string showcaseName, double showcaseVolume)
        {
            Showcase showcase = new Showcase(showcaseName, showcaseVolume);
            UnitOfWork.ShowcaseRepository.Add(showcase);
            showcase.Id = UnitOfWork.ShowcaseRepository.GetCount();
        }

        public void DeleteShowcase(int showcaseId)
        {
            if (CheckShowcaseAvailability() && GetShowcaseCount() >= showcaseId)
            {
                if (CheckShowcaseCount(showcaseId))
                {
                    UnitOfWork.ShowcaseRepository.DeleteById(showcaseId);
                    NotifyService.RaiseDeleteShowcaseIsDone();
                    var showcase = from s in UnitOfWork.ShowcaseRepository.GetAll()
                                   select s;
                    for (int i = 0; i < GetShowcaseCount(); i++)
                    {
                        showcase.ElementAtOrDefault(i).Id = i + 1;
                    }
                }
            }
            else
            {
                throw new IdNotFoundException("Id not found");
                //NotifyService.RaiseSearchProductIdIsNotSuccessful();
            }
        }

        public void PlaceProductOnShowcase(int productId, int showcaseId)
        {
            if (GetShowcaseCount() >= showcaseId && ProductController.GetProductCount() >= productId)
            {
                if (ProductController.CheckProductAvailability() && CheckShowcaseAvailability())
                {
                    if (CheckShowcaseVolumeOverflow(showcaseId, productId))
                    {
                        var selectProduct = ProductController.GetProduct(productId);
                        var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId);
                        selectShowcase.UnitOfWork.ProductOnShowcaseRepository.Add(selectProduct);
                        SumShowcaseVolume(showcaseId, productId);
                        ProductController.DeleteProduct(productId);
                        selectProduct.IdInShowcase = selectShowcase.GetProductCount();
                        selectProduct.IdShowcase = showcaseId;
                    }
                }
            }
            else
            {
                throw new IdNotFoundException("Id not found");
            }
        }

        public void DeleteProductOnShowcase(int showcaseId, int productId)
        {
            if (CheckShowcaseAvailability() && GetShowcaseCount() >= showcaseId)
            {
                var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId);
                var selectProduct = selectShowcase.GetProduct(productId);

                if (CheckProductOnCurrentShowcase(showcaseId))
                {
                    selectShowcase.UnitOfWork.ProductOnShowcaseRepository.DeleteById(productId);
                    NotifyService.RaiseDeleteProductIsDone();
                    selectShowcase.VolumeCount -= selectProduct.Volume;
                    var products = from p in selectShowcase.UnitOfWork.ProductOnShowcaseRepository.GetAll()
                                   select p;
                    for (int i = 0; i < products.Count(); i++)
                    {
                        products.ElementAtOrDefault(i).IdInShowcase = i + 1;
                    }
                }
            }
            else
            {
                throw new IdNotFoundException("Id not found");
            }
        }

        public void EditeShowcase(int showcaseId, string showcaseName, double showcaseVolume)
        {
            if (CheckShowcaseAvailability() && GetShowcaseCount() >= showcaseId)
            {
                var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId); 
                var products = from p in selectShowcase.UnitOfWork.ProductOnShowcaseRepository.GetAll()
                               select p;
                if (products.Count() != 0)
                {
                    NotifyService.RaiseDeleteError();
                }
                else
                {
                    selectShowcase.Name = showcaseName;
                    selectShowcase.Volume = showcaseVolume; 
                    NotifyService.RaiseEditShowcaseIsDone();
                }
                
            }
            else
            {
                NotifyService.RaiseSearchProductIdIsNotSuccessful();
            }
        }

        public void EditeProductOnShowcase(int productId, int showcaseId, string productName, double productVolume)
        {
            if (CheckShowcaseAvailability() && GetShowcaseCount() >= showcaseId)
            {
                if (CheckProductOnCurrentShowcase(showcaseId))
                {
                    if (productVolume <= GetShowcaseFreeSpace(showcaseId))
                    {
                        var selectShowcase = UnitOfWork.ShowcaseRepository.GetById(showcaseId);
                        var selectProduct = selectShowcase.UnitOfWork.ProductOnShowcaseRepository.GetById(productId);
                        selectProduct.Name = productName;
                        selectProduct.Volume = productVolume;
                    }
                    else
                    {
                        NotifyService.RaiseVolumeErrorMessage();
                    }
                }
            }
            else
            {
                NotifyService.RaiseSearchProductIdIsNotSuccessful();
            }
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
