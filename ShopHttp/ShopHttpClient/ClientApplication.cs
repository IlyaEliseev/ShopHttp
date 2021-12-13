using System;
using ShopHttp.ShopHttpClient.Controllers;
using ShopHttp.ShopHttpClient.Services;
using ShopHttp.ShopHttpServer.DAL;

namespace ShopHttp.ShopHttpClient
{
    public class ClientApplication
    {
        public ClientApplication(IProductHttpRequestController productHttpController, IProductArchiveHttpRequestController productArchiveHttpController, IShowcaseHttpRequestController showcaseHttpController,
                                  CheckService checkService)
        {
            ProductHttpController = productHttpController;
            ProductArchiveHttpController = productArchiveHttpController;
            ShowcaseHttpController = showcaseHttpController;
            CheckService = checkService;
        }

        public IProductHttpRequestController ProductHttpController { get; }
        public IProductArchiveHttpRequestController ProductArchiveHttpController { get; }
        public IShowcaseHttpRequestController ShowcaseHttpController { get; }
        public CheckService CheckService { get; }

        public void Run()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            bool isContinue = true;
            while (isContinue)
            {
                ClientApplication.ShowUserMenu();
                Console.WriteLine();
                Console.WriteLine("Input command: ");
                string input = Console.ReadLine();
                bool succses = int.TryParse(input, out int command);
                if (succses == false || command > Enum.GetNames(typeof(InputCommands)).Length)
                {
                    Messages.SetRedColor("Wrong command!");
                }

                else
                {
                    if (command == (int)InputCommands.CreateProduct)
                    {
                        string productName = CheckService.CheckName(GetName());
                        double productVolume = CheckService.CheckVolume(GetVolume());
                        ProductHttpController.CreateProduct(productName, productVolume);
                    }

                    if (command == (int)InputCommands.EditeProduct)
                    {
                        int productId = CheckService.CheckId(GetProductId());
                        string productName = CheckService.CheckName(GetName());
                        double productVolume = CheckService.CheckVolume(GetVolume());
                        ProductHttpController.EditProduct(productId, productName, productVolume);
                    }

                    if (command == (int)InputCommands.DeleteProduct)
                    {
                        int productId = CheckService.CheckId(GetProductId());
                        ProductHttpController.DeleteProduct(productId);
                    }

                    if (command == (int)InputCommands.GetProductInformation)
                    {
                        ProductHttpController.GetProductInformation();
                    }

                    if (command == (int)InputCommands.ShowAllShowcases)
                    {
                        ShowcaseHttpController.GetShowcaseInformation();
                    }

                    if (command == (int)InputCommands.CreateShowcase)
                    {
                        string showcaseName = CheckService.CheckName(GetName());
                        double showcaseVolume = CheckService.CheckVolume(GetVolume());
                        ShowcaseHttpController.CreateShowcase(showcaseName, showcaseVolume);
                    }

                    if (command == (int)InputCommands.DeleteShowcase)
                    {
                        int showcaseId = CheckService.CheckId(GetShowcaseId());
                        ShowcaseHttpController.DeleteShowcase(showcaseId);
                    }

                    if (command == (int)InputCommands.PlaceProductOnShowcase)
                    {
                        int showcaseId = CheckService.CheckId(GetShowcaseId());
                        int productId = CheckService.CheckId(GetProductId());
                        ShowcaseHttpController.PlaceProductOnShowcase(productId, showcaseId);
                    }

                    if (command == (int)InputCommands.DeleteProductOnShowcase)
                    {
                        int showcaseId = CheckService.CheckId(GetShowcaseId());
                        int productId = CheckService.CheckId(GetProductId());
                        ShowcaseHttpController.DeleteProductOnShowcase(showcaseId, productId);
                    }

                    if (command == (int)InputCommands.EditShowcase)
                    {
                        int showcaseId = CheckService.CheckId(GetShowcaseId());
                        string showcaseName = CheckService.CheckName(GetName());
                        double showcaseVolume = CheckService.CheckVolume(GetVolume());
                        ShowcaseHttpController.EditeShowcase(showcaseId, showcaseName, showcaseVolume);
                    }

                    if (command == (int)InputCommands.EditProductOnShowcase)
                    {
                        int showcaseId = CheckService.CheckId(GetShowcaseId());
                        int productId = CheckService.CheckId(GetProductId());
                        string productName = CheckService.CheckName(GetName());
                        double productVolume = CheckService.CheckVolume(GetVolume());
                        ShowcaseHttpController.EditeProductOnShowcase(productId, showcaseId, productName, productVolume);
                    }

                    if (command == (int)InputCommands.ArchivateProduct)
                    {
                        var showcaseId = CheckService.CheckId(GetShowcaseId());
                        int productId = CheckService.CheckId(GetProductId());
                        ProductArchiveHttpController.ArchivateProduct(productId, showcaseId);
                    }

                    if (command == (int)InputCommands.UnArchivateProduct)
                    {
                        var productId = CheckService.CheckId(GetProductId());
                        ProductArchiveHttpController.UnArchivateProduct(productId);
                    }

                    if (command == (int)InputCommands.DeleteArchiveProduct)
                    {
                        int productId = CheckService.CheckId(GetProductId());
                        ProductArchiveHttpController.DeleteArchiveProduct(productId);
                    }

                    if (command == (int)InputCommands.GetArchiveInformation)
                    {
                        ProductArchiveHttpController.GetArchiveInformation();
                    }

                    if (command == (int)InputCommands.EXITApplication)
                    {
                        isContinue = false;
                    }
                }
            }
        }

        public static string GetName()
        {
            Console.WriteLine("Input name:");
            string name = Console.ReadLine();
            return name;
        }

        public static string GetVolume()
        {
            Console.WriteLine("Input volume:");
            string volume = Console.ReadLine();
            return volume;
        }

        public static string GetProductId()
        {
            Console.WriteLine("Input product Id: ");
            string Id = Console.ReadLine();
            return Id;
        }

        public static string GetShowcaseId()
        {
            Console.WriteLine("Input showcase Id: ");
            string Id = Console.ReadLine();
            return Id;
        }

        public static void ShowUserMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Showcase command: ");
            Console.WriteLine($"Press [{(int)InputCommands.CreateShowcase}] to CREATE showcase");
            Console.WriteLine($"Press [{(int)InputCommands.ShowAllShowcases}] to SHOW all showcases");
            Console.WriteLine($"Press [{(int)InputCommands.PlaceProductOnShowcase}] to PLACE product");
            Console.WriteLine($"Press [{(int)InputCommands.EditProductOnShowcase}] to EDIT product on showcase");
            Console.WriteLine($"Press [{(int)InputCommands.DeleteProductOnShowcase}] to DELETE product on showcase");
            Console.WriteLine($"Press [{(int)InputCommands.EditShowcase}] to EDIT showcase");
            Console.WriteLine($"Press [{(int)InputCommands.DeleteShowcase}] to DELETE showcase");
            Console.WriteLine();
            Console.WriteLine("Product command: ");
            Console.WriteLine($"Press [{(int)InputCommands.CreateProduct}] to CREATE product");
            Console.WriteLine($"Press [{(int)InputCommands.EditeProduct}] to EDIT product");
            Console.WriteLine($"Press [{(int)InputCommands.DeleteProduct}] to DELETE product");
            Console.WriteLine($"Press [{(int)InputCommands.GetProductInformation}] to GET product information");
            Console.WriteLine();
            Console.WriteLine("Archive command: ");
            Console.WriteLine($"Press [{(int)InputCommands.ArchivateProduct}] to ARCHIVATE product");
            Console.WriteLine($"Press [{(int)InputCommands.UnArchivateProduct}] to UNARCHIVATE product");
            Console.WriteLine($"Press [{(int)InputCommands.DeleteArchiveProduct}] to DELETE archive product");
            Console.WriteLine($"Press [{(int)InputCommands.GetArchiveInformation}] to GET archive information");
            Console.WriteLine();
            Console.WriteLine($"Press [{(int)InputCommands.EXITApplication}] to EXIT the application");
            Console.WriteLine();
        }
    }
}
