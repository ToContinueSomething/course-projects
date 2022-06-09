using System;
using System.Collections.Generic;

namespace Seller
{
    class Program
    {
        static void Main(string[] args)
        {
            Shop shop = new Shop();
            shop.StartSelling();
        }
    }

    class Shop
    {
        private Seller _seller;
        private Player _player;

        public Shop()
        {
            int sellerSeedMoney = 70000;
            _seller = new Seller(sellerSeedMoney);
            _seller.AddProducts();

            int playerSeedMoney = 90000;
            _player = new Player(playerSeedMoney);
        }

        public void StartSelling()
        {
            bool isWork = true;

            while (isWork)
            {
                Console.WriteLine("Product - показать товар\nShow - посмотреть свою сумку" +
                    "\nBuy - купить товар\nSell - продать товар\nExit - Уйти");

                switch (Console.ReadLine())
                {
                    case "Product":
                        _seller.ShowProducts();
                        break;
                    case "Show":
                        _player.ShowProducts();
                        break;
                    case "Buy":
                        _seller.SellProduct(_player);
                        break;
                    case "Sell":
                        _player.SellProduct(_seller);
                        break;
                    case "Exit":
                        isWork = false;
                        break;
                }
            }
        }
    }

    abstract class Character
    {
        private List<Product> _bag;
        private int _money;

        public Character(int money)
        {
            _bag = new List<Product>();
            _money = money;
        }

        public void AddProducts()
        {
            _bag.Add(new Product("PC"));
            _bag.Add(new Product("Iphone"));
            _bag.Add(new Product("Notebook"));
        }

        public void ShowProducts()
        {
            Console.WriteLine("На счету - " + _money);

            foreach (var product in _bag)
            {
                product.ShowInfo();
            }
        }

        public void SellProduct(Character character)
        {
            Product soldProduct = null;
            int priceForProduct;
            string desiredProduct = Console.ReadLine();

            foreach (var product in _bag)
            {
                if (desiredProduct == product.Label && character.EnoughMoney(product.Price))
                {
                    Console.WriteLine("Сделка прошла успешно");
                    priceForProduct = product.Price;

                    character.ToPay(priceForProduct);
                    character.PutProduct(product);

                    _money += priceForProduct;
                    soldProduct = product;
                }
            }

            _bag.Remove(soldProduct);
        }

        public bool EnoughMoney(int priceForProduct)
        {
            return _money >= priceForProduct;
        }

        public void PutProduct(Product product)
        {
            _bag.Add(product);
        }

        public void ToPay(int priceForProduct)
        {
            _money -= priceForProduct;
        }
    }

    class Seller : Character
    {
        public Seller(int money) : base(money) { }
    }

    class Player : Character
    {
        public Player(int money) : base(money) { }
    }

    class Product
    {
        private static Random _random;
        
        public int Price { get; private set; }
        public string Label { get; private set; }

        static Product()
        {
            _random = new Random();
        }

        public Product(string label)
        {
            Label = label;
            int minPrice = 10000;
            int maxPrice = 50000;
            Price = _random.Next(minPrice, maxPrice);
        }

        public void ShowInfo()
        {
            Console.WriteLine(Label + "|Цена - " + Price);
        }
    }
}