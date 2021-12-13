using ShopHttp.ShopHttpServer.DAL;
using ShopHttp.ShopHttpServer.Models;
using ShopHttp.ShopHttpServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopHttp.ShopHttpServer.Controllers
{
    public class ProductArchiveController : IProductArchiveController
    {
        public ProductArchiveController(NotifyService notifyService, IShowcaseController showcaseController, CheckService checkService)
        {
            NotifyService = notifyService;
            ShowcaseController = showcaseController;
            CheckService = checkService;
            UnitOfWork = new UnitOfWork();
        }

        public NotifyService NotifyService { get; }
        public IShowcaseController ShowcaseController { get; }
        public CheckService CheckService { get; }
        public IUnitOfWork UnitOfWork { get; }

        public void ArchivateProduct(int productId, int showcaseId)
        {
            if (ShowcaseController.CheckShowcaseAvailability() && ShowcaseController.GetShowcaseCount() >= showcaseId 
                && ShowcaseController.GetProductCountOnShowcase(showcaseId) >= productId)
            {
                if (ShowcaseController.CheckProductOnCurrentShowcase(showcaseId))
                {
                    var selectShowcase = ShowcaseController.GetShowcaseById(showcaseId);
                    var selectProduct = selectShowcase.GetProduct(productId);
                    UnitOfWork.ArchiveRepository.Add(selectProduct);
                    selectProduct.IdInArchive = GetArchiveProductCount();
                    selectProduct.TimeToArchive = DateTime.Now;
                    ShowcaseController.DeleteProductOnShowcase(showcaseId, productId);
                    //NotifyService.RaiseArchivateProductIsDone();
                }
            }
            else
            {
                throw new IdNotFoundException("Id not found");
                //NotifyService.RaiseSearchProductIdIsNotSuccessful();
            }
        }

        public bool CheckArchiveAvailability()
        {
            if (GetArchiveProductCount() == 0)
            {
                //NotifyService.RaiseArchiveIsEmpty();
                return false;
            }
            else
            {
                return true;
            }
        }

        public void DeleteArchiveProduct(int productId)
        {
            if (CheckArchiveAvailability() && GetArchiveProductCount() >= productId)
            {
                UnitOfWork.ArchiveRepository.DeleteById(productId);
                //NotifyService.RaiseDeleteArchiveProductIsDone();
                var products = from p in UnitOfWork.ArchiveRepository.GetAll()
                               select p;
                for (int i = 0; i < UnitOfWork.ArchiveRepository.GetCount(); i++)
                {
                    products.ElementAtOrDefault(i).IdInArchive = i + 1;
                }
            }
            else
            {
                throw new IdNotFoundException("Id not found");
                //NotifyService.RaiseSearchProductIdIsNotSuccessful();
            }
        }

        //public void GetArchiveInformation()
        //{
        //    if (CheckArchiveAvailability())
        //    {
        //        Console.WriteLine("Archive:");
        //        var archive = from a in UnitOfWork.ArchiveRepository.GetAll()
        //                      select a;
        //        foreach (var product in archive)
        //        {
        //            Console.WriteLine($"Id: {product.IdInArchive} | Name product: {product.Name} | Volume product: {product.Volume} | Time to create: {product.TimeToCreate} | Time to archive: {product.TimeToArchive}");
        //        }
        //    }
        //}

        public int GetArchiveProductCount()
        {
            return UnitOfWork.ArchiveRepository.GetCount();
        }

        public void UnArchivateProduct(int productId)
        {
            if (CheckArchiveAvailability() && GetArchiveProductCount() >= productId)
            {
                var selectProduct = UnitOfWork.ArchiveRepository.GetById(productId);
                var selectShowcase = ShowcaseController.GetShowcaseById(selectProduct.IdShowcase);
                selectShowcase.UnitOfWork.ProductOnShowcaseRepository.Add(selectProduct);
                selectProduct.IdInShowcase = selectShowcase.GetProductCount();
                UnitOfWork.ArchiveRepository.DeleteById(productId);
                //NotifyService.RaiseUnArchivateProductIsDone();
                var products = from p in UnitOfWork.ArchiveRepository.GetAll()
                               select p;
                for (int i = 0; i < UnitOfWork.ArchiveRepository.GetCount(); i++)
                {
                    products.ElementAtOrDefault(i).IdInArchive = i + 1;
                }
            }
            else
            {
                throw new IdNotFoundException("Id not found");
                //NotifyService.RaiseSearchProductIdIsNotSuccessful();
            }
        }

        public IEnumerable<Product> GetArchiveProducts()
        {
            return UnitOfWork.ArchiveRepository.GetAll();
        }
    }
}
