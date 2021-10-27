using System;
using System.Collections.Generic;
using System.Linq;

namespace Shops
{
    public class Shop : ICanBuy
    {
        public float Money { get; private set; }
        public List<Product> ProductsToBuy { get; private set; }


        private string _nameOfShop;
        private string _address;
        private Guid _id = new Guid();
        public Dictionary<string, Product> AmountOfGoods { get; private set; }

        public void DecreaseMoney(float money)
        {
            this.Money -= money;
        }

        public void SetProductsToBuy(Product[] products)
        {
            ProductsToBuy = new List<Product>(products);
        }

        public Shop(string nameOfShop = "Ilia's shop", string address = "Street 5", float startCapital = 500f)
        {
            Money = startCapital;
            this._nameOfShop = nameOfShop;
            this._address = address;

            AmountOfGoods = new Dictionary<string, Product>();
        }

        public int GetAmountOfGoodWithThisName(string name)
        {
            if (AmountOfGoods[name] != null)
            {
                int amount = AmountOfGoods[name].Amount;
                return amount;
            }

            return 0;
        }

        public Guid GetID()
        {
            return _id;
        }

        public void SetPriceForGoodsWithName(string name, float price)
        {
            if (!AmountOfGoods.ContainsKey(name)) throw new Exception("There is no such good");
            AmountOfGoods[name].ChangePrice(price);
        }

        public void AddGoods(params Product[] newGoods)
        {
            foreach (Product product in newGoods)
            {
                var name = product.Name;
                var amount = product.Amount;
                var price = product.Price;
                if (AmountOfGoods.ContainsKey(name))
                {
                    if (amount > 0) AmountOfGoods[name].AddAmount(amount);
                    AmountOfGoods[name].ChangePrice(price);
                }
                else
                {
                    AmountOfGoods.Add(name, product);
                }
            }
        }

        public Product GetGood(string nameOfGood)
        {
            if (!AmountOfGoods.ContainsKey(nameOfGood)) return null;
            var product = AmountOfGoods[nameOfGood];
            Product good = new Product(product);
            return good;
        }

        public void ServeGood(ICanBuy customer)
        {
            float totalSum = customer.ProductsToBuy.Sum(t => GetGood(t.Name).Price * t.Amount);
            var goods = customer.ProductsToBuy;
            if (totalSum > customer.Money) throw new Exception("Not enough money for purchase");
            foreach (var good in goods)
            {
                var name = good.Name;
                var amountOfGood = good.Amount;
                if (!AmountOfGoods.ContainsKey(name))
                {
                    throw new Exception($"there is no products with name: {name}");
                }


                if (AmountOfGoods[name].Amount < amountOfGood)
                    throw new Exception("Not enough goods in shop for buying");
                AmountOfGoods[name].DecreaseAmount(amountOfGood);
            }

            Money += totalSum;
            customer.DecreaseMoney(totalSum);
        }

        public void UpdateGoods()
        {
            AddGoods(ProductsToBuy.ToArray());
        }

        public float CalculateSum(Product[] productsToBuy)
        {
            float sum = 0;
            foreach (var product in productsToBuy)
            {
                if (!AmountOfGoods.ContainsKey(product.Name))
                    throw new Exception($"There is no{product.Name} in {_nameOfShop}");
                var amount = AmountOfGoods[product.Name].Amount;
                var price = AmountOfGoods[product.Name].Price;
                sum += amount * price;
            }

            return sum;
        }

        public Shop FindCheapestCollectionOfGoodInShoops(Shop[] shops)
        {
            float cheapestSPrice = float.MaxValue;
            var productsToBuy = this.ProductsToBuy;
            Shop shopToRemember = new Shop();
            foreach (var shop in shops)
            {
                float price = shop.CalculateSum(productsToBuy.ToArray());
                if (price <= cheapestSPrice)
                {
                    cheapestSPrice = price;
                    shopToRemember = shop;
                }
            }

            return shopToRemember;
        }
    }

    public class ShopBuilder
    {
        public float Money { get; private set; }
        public List<Product> ProductsToBuy { get; private set; }
        private string _nameOfShop;
        private string _address;

        public ShopBuilder WithMoney(float money)
        {
            Money = money;
            return this;
        }

        public ShopBuilder WithProducts(params Product[] newGoods)
        {
            ProductsToBuy = new List<Product>(newGoods);
            return this;
        }

        public ShopBuilder WithProducts(List<Product> newGoods)
        {
            ProductsToBuy = newGoods;
            return this;
        }

        public ShopBuilder WithName(string name)
        {
            _nameOfShop = name;
            return this;
        }

        public ShopBuilder WithAddress(string addres)
        {
            _address = addres;
            return this;
        }

        public Shop Build()
        {
            return new Shop(_nameOfShop, _address, Money);
        }

        public static implicit operator Shop(ShopBuilder builder)
        {
            return builder.Build();
        }

        public ShopBuilder ToBuild()
        {
            ShopBuilder shopBuilder = new ShopBuilder();
            return shopBuilder.WithName(_nameOfShop).WithMoney(Money).WithAddress(_address);
        }
    }
}