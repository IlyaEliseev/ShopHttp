using System;

namespace ShopHttp.ShopHttpClient
{
    internal class Messages
    {
        public static void ProductIsCreate()
        {
            SetGreenColor("Product is create!");
        }

        public static void ProductIsPlace()
        {
            SetGreenColor("Product is place on showcase!");
        }

        public static void ProductIsEdit()
        {
            SetGreenColor("Product is edit!");
        }

        public static void ProductIsDelete()
        {
            SetGreenColor("Product is delete!");
        }

        public static void ShowcaseIsCreate()
        {
            SetGreenColor("Showcase is create!");
        }

        public static void ShowcaseIsEdit()
        {
            SetGreenColor("Showcase is edit!");
        }

        public static void ShowcaseIsDelete()
        {
            SetGreenColor("Showcase is delete!");
        }

        public static void ProductArchivate()
        {
            SetGreenColor("Product is archivate!");
        }

        public static void ProductUnArchivate()
        {
            SetGreenColor("Product is unarchivate!");
        }

        public static void ArchiveProductDelete()
        {
            SetGreenColor("Archive product is delete!");
        }

        public static void ArchiveEmpty()
        {
            SetRedColor("Archive is empty!");
        }

        public static void CountIsEmptyInformation()
        {
            SetRedColor("Showcase or product storage empty!");
        }

        public static void IdNotFound()
        {
            SetRedColor("Id not found!");
        }

        public static void DeliteShowcaseMessage()
        {
            SetRedColor("Showcase is not empty!");
        }

        public static void VolumeErrorMessage()
        {
            SetRedColor("Showcase not enough space!");
        }

        public static void ShowNotProductOnShowcase()
        {
            SetRedColor("No product on Showcase!");
        }

        public static void SetRedColor(string messege)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{messege}");
            Console.ResetColor();
        }

        public static void SetGreenColor(string messege)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{messege}");
            Console.ResetColor();
        }
    }
}
