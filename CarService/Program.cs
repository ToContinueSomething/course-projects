using System;
using System.Collections.Generic;

namespace CarService
{
    class Program
    {
        static void Main(string[] args)
        {
            CarService carService = new CarService();
            carService.StartWork();
        }
    }

    class CarService
    {
        private Queue<Client> _clients;
        private PartsWarehouse _partsWarehouse;

        private int _priceForFixing;
        private int _money;

        public bool IsBankruptcy => _money < 0;

        public CarService()
        {
            _clients = new Queue<Client>();
            _partsWarehouse = new PartsWarehouse();

            _money = 5000;
            _priceForFixing = 5000;

            int countClients = 10;
            CreateNewClients(countClients);
        }

        public void StartWork()
        {
            while (_clients.Count > 0 && IsBankruptcy == false)
            {
                Console.WriteLine($"У сервиса на счету:{_money}\n");

                Client client = _clients.Dequeue();
                int priceForAll = CalculationPrice(client.Part);

                Console.WriteLine($"Приехал клиент.У него сломанная деталь {client.Part.Label}|Цена за починку:{priceForAll}\n");

                _partsWarehouse.ShowParts();

                if (client.EnoughMoney(priceForAll))
                {
                    if (TryInstallPart(client))
                    {
                        client.ToPay(priceForAll);
                        _money += priceForAll;
                    }
                }

                Console.WriteLine("Для продолжения нажмите клавишу...");
                Console.ReadKey();
                Console.Clear();
            }

            if (IsBankruptcy)
                Console.WriteLine("Сервис обанкротился");
        }

        private int CalculationPrice(Part part)
        {
            return part.Price + _priceForFixing;
        }

        private bool TryInstallPart(Client client)
        {
            Part replacementPart = _partsWarehouse.GetPart();

            if (ContainsPart(replacementPart))
            {
                if (client.ContainsCorrectPart(replacementPart.Label))
                {
                    client.InstallPart(replacementPart);
                    Console.WriteLine("Починка была завершена.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Была поставлена неподходящая деталь.");
                    _money -= _priceForFixing;
                    return false;
                }
            }

            return false;
        }

        private bool ContainsPart(Part part)
        {
            return part != null;
        }

        private void CreateNewClients(int count)
        {
            int clientSeedMoney = 15000;

            for (int i = 0; i < count; i++)
            {
                _clients.Enqueue(new Client(clientSeedMoney));
            }
        }
    }

    class Client
    {
        private int _money;
        private static Random _random;
        public Part Part { get; private set; }

        static Client()
        {
            _random = new Random();
        }

        public Client(int money)
        {
            List<Part> parts = new List<Part>();
            _money = money;

            for (Label label = 0; (int)label < Enum.GetValues(typeof(Label)).Length; label++)
            {
                parts.Add(new Part(label));
            }

            Part = parts[_random.Next(0, parts.Count)];
        }

        public void ToPay(int priceForAll)
        {
            _money -= priceForAll;
        }

        public bool EnoughMoney(int priceForAll)
        {
            return _money >= priceForAll;
        }

        public void InstallPart(Part part)
        {
            Part = part;
        }

        public bool ContainsCorrectPart(Label labelPart)
        {
            return Part.Label == labelPart;
        }
    }

    class PartsWarehouse
    {
        private List<Box> _boxes;

        public PartsWarehouse()
        {
            _boxes = new List<Box>();
            AddParts();
        }

        public Part GetPart()
        {
            int index = GetCorrectIndex();
            return _boxes[index].PickUpPart();
        }

        public void ShowParts()
        {
            for (int i = 0; i < _boxes.Count; i++)
            {
                Console.Write(i + "|");
                _boxes[i].ShowParts();
            }
        }

        private int GetCorrectIndex()
        {
            bool isWork = true;
            int number = int.MaxValue;

            while (isWork)
            {
                string value = Console.ReadLine();
                bool success = int.TryParse(value, out number);
                int incorrectIndex = 0;

                if (success && number >= incorrectIndex && number < _boxes.Count)
                    isWork = false;
            }

            return number;
        }     

        private void AddParts()
        {
            for (Label label = 0; (int)label < Enum.GetValues(typeof(Label)).Length; label++)
            {
                _boxes.Add(new Box(new Part(label)));
            }
        }
    }

    enum Label
    {
        Engine,
        Gearbox,
        Suspension,
        Clutch
    }

    class Part
    {
        private static Random _random;

        public Label Label { get; private set; }
        public int Price { get; private set; }

        static Part()
        {
            _random = new Random();
        }

        public Part(Label label)
        {
            Label = label;

            int minPrice = 5000;
            int maxPrice = 7000;
            Price = _random.Next(minPrice, maxPrice);
        }

        public void ShowInfo()
        {
            Console.Write(Label);
        }
    }

    class Box
    {
        private static Random _random;
        private Part _part;
        private int _countParts;

        public bool IsEmpty => _countParts <= 0;

        static Box()
        {
            _random = new Random();
        }

        public Box(Part part)
        {
            _part = part;

            int minCountParts = 5;
            int maxCountParts = 9;
            _countParts = _random.Next(minCountParts, maxCountParts);
        }

        public Part PickUpPart()
        {
            if (IsEmpty == false)
            {
                _countParts--;
                return _part;
            }
            else
            {
                return null;
            }
        }

        public void ShowParts()
        {
            _part.ShowInfo();
            Console.WriteLine("|Количество:" + _countParts);
        }
    }
}
