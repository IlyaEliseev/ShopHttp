using ShopHttp.ShopHttpServer.Models;
using System.Collections.Generic;

namespace ShopHttp.ShopHttpServer.Controllers
{
    public interface IShowcaseController
    {
        void CreateShowcase(string nameShowcase, double volumeShowcase);
        void DeleteShowcase(int showcaseId);
        void PlaceProductOnShowcase(int productId, int showcaseId);
        void DeleteProductOnShowcase(int showcaseId, int productId);
        void EditeShowcase(int showcaseId, string showcaseName, double showcaseVolume);
        void EditeProductOnShowcase(int productId, int showcaseId, string productName, double productVolume);
        bool CheckShowcaseAvailability();
        bool CheckShowcaseCount(int showcaseId);
        bool CheckShowcaseVolumeOverflow(int showcaseId, int productId);
        double GetShowcaseFreeSpace(int showcaseId);
        int GetShowcaseCount();
        void SumShowcaseVolume(int showcaseId, int productId);
        bool CheckProductOnCurrentShowcase(int showcaseId);
        Showcase GetShowcaseById(int showcaseId);
        int GetProductCountOnShowcase(int showcaseId);
        IEnumerable<Showcase> GetShowcases();
    }
}
