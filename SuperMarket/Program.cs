using System;
using System.Collections.Generic;

namespace SuperMarket
{
    class Program
    {
        static void Main(string[] args)
        {
            SuperMarket superMarket = new SuperMarket();
            superMarket.StartWork();
        }
    }

    class SuperMarket
    {
        private static Random _random;

        private List<Product> _products;
        private List<Product> _shoppingCart;
        private Queue<Client> _clients;

        private int _money;

        static SuperMarket()
        {
            _random = new Random();
        }

        public SuperMarket()
        {
            int superMarketSeedMoney = 5000;
            _money = superMarketSeedMoney;

            _clients = new Queue<Client>();
            _shoppingCart = new List<Product>();
            _products = new List<Product>();
            AddProducts();

            int countClients = 5;
            CreateNewClients(countClients);
        }

        public void StartWork()
        {
            while (_clients.Count > 0)
            {
                Console.WriteLine($"Супер Маркет.На счету {_money}");
                Client client = _clients.Dequeue();
                FillCartClient();

                int priceForProducts;
                priceForProducts = CalculateThePrice();
                Console.WriteLine($"Цена за все продукты составляет {priceForProducts}");
                Console.ReadKey();

                if (client.EnoughMoney(priceForProducts))
                {
                    ServeClient(client);
                }
                else
                {
                    while (client.EnoughMoney(priceForProducts) == false)
                    {
                        Console.WriteLine("Не хватает денег, придется убрать один товар");
                        _shoppingCart.RemoveAt(_random.Next(0, _shoppingCart.Count));
                        priceForProducts = CalculateThePrice();
                    }

                    ServeClient(client);
                }

                _money += priceForProducts;

                _shoppingCart = new List<Product>();
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void FillCartClient()
        {
            bool isWork = true;

            while (isWork)
            {
                Console.WriteLine("В СуперМаркете есть:\n");
                ShowProducts();

                Console.WriteLine("\nВведите 'stop' чтоб перестать заполнять корзину.");
                Console.Write("\nПродукт:");
                string desiredProduct = Console.ReadLine();

                if (desiredProduct == "stop")
                {
                    isWork = false;
                }
                else
                {
                    if (ContainsCorrectProduct(desiredProduct))                      
                        _shoppingCart.Add(new Product(desiredProduct));                  
                    else 
                        Console.WriteLine("Такого продукта нет.");
                }

                Console.ReadKey();
                Console.Clear();
            }
        }

        private int CalculateThePrice()
        {
            int priceForProducts = 0;
            Console.WriteLine("Продукты клиента:");

            foreach (var product in _shoppingCart)
            {
                product.ShowInfo();
                priceForProducts += product.Price;
            }

            return priceForProducts;
        }

        private bool ContainsCorrectProduct(string desiredProduct)
        {
            bool isCheck = true;

            foreach (var product in _products)
            {
                if (desiredProduct == product.Label)
                {
                    isCheck = true;
                    break;
                }
                else
                {
                    isCheck = false;
                }
            }

            return isCheck;
        }

        private void ServeClient(Client client)
        {
            foreach (var product in _shoppingCart)
            {
                client.PickUpProduct(product);
            }

            Console.WriteLine("Клиент был обслужан.");
        }

        private void ShowProducts()
        {
            foreach (var product in _products)
            {
                product.ShowInfo();
            }
        }

        private void AddProducts()
        {
            _products.Add(new Product("Колбаса"));
            _products.Add(new Product("Сыр"));
            _products.Add(new Product("Напиток"));
            _products.Add(new Product("Овощи"));
            _products.Add(new Product("Фрукты"));
        }

        private void CreateNewClients(int count)
        {
            int minClientSeedMoney = 1000;
            int maxClientSeedMoney = 3000;

            for (int i = 0; i < count; i++)
            {
                _clients.Enqueue(new Client(_random.Next(minClientSeedMoney, maxClientSeedMoney)));
            }
        }
    }

    class Client
    {
        private List<Product> _bag;
        private int _money;

        public Client(int money)
        {
            _bag = new List<Product>();
            _money = money;
        }

        public bool EnoughMoney(int priceForProducts)
        {
            return _money >= priceForProducts;
        }

        public void ToPay(int priceForProducts)
        {
            _money -= priceForProducts;
        }

        public void PickUpProduct(Product product)
        {
            _bag.Add(product);
        }
    }

    class Product
    {
        private static Random _random;

        public string Label { get; private set; }
        public int Price { get; private set; }

        static Product()
        {
            _random = new Random();
        }

        public Product(string label)
        {
            Label = label;

            int minPrice = 100;
            int maxPrice = 1000;
            Price = _random.Next(minPrice, maxPrice);
        }

        public void ShowInfo()
        {
            Console.WriteLine(Label + "|Цена:" + Price);
        }
    }
}