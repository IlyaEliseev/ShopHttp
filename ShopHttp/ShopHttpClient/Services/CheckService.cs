using System;

namespace ShopHttp.ShopHttpClient.Services
{
    public class CheckService
    {
        protected internal int CheckId(string id)
        {
            int verifiableId;
            bool isContinue = true;
            do
            {
                bool succses = int.TryParse(id, out verifiableId);
                if (succses == false || verifiableId == 0)
                {
                    Messages.SetRedColor("Id not found!");
                    Console.WriteLine("Input Id: ");
                    id = Console.ReadLine();
                }
                else
                {
                    isContinue = false;
                }
            }while (isContinue);
            
            return verifiableId;
        }

        public double CheckVolume(string volume)
        {
            double verifiableVolume;
            bool isContinue = true;
            do
            {
                bool succses = double.TryParse(volume, out verifiableVolume);
                
                if (succses == false)
                {
                    Messages.SetRedColor("Volume is uncorrect!");
                    Console.WriteLine("Input volume: ");
                    volume = Console.ReadLine();
                }
                else
                {
                    isContinue = false;
                }
            } while (isContinue);
            return verifiableVolume;
        }

        public string CheckName(string name)
        {
            bool isContinue = true;
            do
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    Messages.SetRedColor("Name is uncorrect!");
                    Console.WriteLine("Input name:");
                    name = Console.ReadLine();
                }
                else
                {
                    isContinue = false;
                }
            } while (isContinue);
            return name;
        }
    }
}
