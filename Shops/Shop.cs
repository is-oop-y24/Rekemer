using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Shops
{
    public class Shop : ICanBuy
    {
        public float Money { get; private set; }
        public List<Product> ProductsToBuy { get; private set; }


        private string _nameOfShop;
        private string _address;
        public Guid _id { get; private set; }
        public Dictionary<string, Product> Goods { get; private set; }

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

            Goods = new Dictionary<string, Product>();
        }

        public int GetAmountOfGoodWithThisName(string name)
        {
            if (Goods[name] != null)
            {
                int amount = Goods[name].Amount;
                return amount;
            }

            return 0;
        }

       

        public void SetPriceForGoodsWithName(string name, float price)
        {
            if (!Goods.ContainsKey(name)) throw new Exception("There is no such good");
            Goods[name].ChangePrice(price);
        }

        public void ImportToShop(List<Product> newGoods)
        {
            float priceOverall = newGoods.Sum(t => t.Price * t.Amount);
            if (priceOverall > Money) throw new Exception("Not enough money to pay for products");
            DecreaseMoney(priceOverall);
            AddGoods(newGoods);
        }

        public void AddGoods(List<Product> newGoods)
        {
            foreach (Product product in newGoods)
            {
                if (product == null)
                {
                    Debug.WriteLine("AddGodds in Shop: adding null product");
                    continue;
                }

                var name = product.Name;
                var amount = product.Amount;
                var price = product.Price;
                if (Goods.ContainsKey(name))
                {
                    if (amount > 0) Goods[name].AddAmount(amount);
                    Goods[name].ChangePrice(price);
                }
                else
                {
                    Goods.Add(name, product);
                }
            }
        }

        public Product GetGood(string nameOfGood)
        {
            if (!Goods.ContainsKey(nameOfGood)) return null;
            var product = Goods[nameOfGood];
            Product good = new Product(product);
            return good;
        }


        public void ServeGood(ICanBuy customer)
        {
            if (customer == null) return;
            float totalSum = customer.ProductsToBuy.Sum(t => Goods[t.Name].Price * Goods[t.Name].Amount);
            var goods = customer.ProductsToBuy;
            var productsToBuy = new List<Product>();
            if (totalSum > customer.Money) throw new Exception("Not enough money for purchase");
            foreach (var good in goods)
            {
                var name = good.Name;
                var amountOfGood = good.Amount;
                if (!Goods.ContainsKey(name))
                {
                    throw new Exception($"there is no products with name: {name}");
                }
                if (Goods[name].Amount < amountOfGood)
                    throw new Exception("Not enough goods in shop for buying");
                good.ChangePrice(Goods[name].Price);
                productsToBuy.Add(good);
            }
            Handle(customer, productsToBuy);
        }

        public void AddMoney(float money)
        {
            Money += money;
        }

        private void Handle(ICanBuy customer, List<Product> products)
        {
            if (customer == null || products == null) return;
            foreach (var product in products)
            {
                if (product == null) continue;
                float totalSum = product.Amount * product.Price;
                if (customer.Money >= totalSum)
                {
                    ProductBuilder productBuilder = new ProductBuilder();
                    var productToAdd = productBuilder.WithAmount(product.Amount).WithName(product.Name)
                        .WithPrice(product.Price);
                    customer.AddProducts(productToAdd);
                    customer.DecreaseMoney(totalSum);
                    Goods[product.Name].DecreaseAmount(product.Amount);
                    AddMoney(totalSum);
                }
            }
        }


        public float CalculateSum(Product[] productsToBuy)
        {
            float sum = 0;
            foreach (var product in productsToBuy)
            {
                if (product == null) continue;
                if (!Goods.ContainsKey(product.Name))
                    throw new Exception($"There is no{product.Name} in {_nameOfShop}");
                var amount = Goods[product.Name].Amount;
                var price = Goods[product.Name].Price;
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
                if (shop == null) continue;
                float price = shop.CalculateSum(productsToBuy.ToArray());
                if (price <= cheapestSPrice)
                {
                    cheapestSPrice = price;
                    shopToRemember = shop;
                }
            }

            return shopToRemember;
        }

        public void AddProducts(Product newGood)
        {
            if (newGood != null)
            {
                Goods[newGood.Name] = newGood;
            }
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