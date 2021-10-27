using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;


namespace Shops
{
    public class Shop : ICanBuy
    {
        public float Money { get; private set; }
        public List<Product> ProductsToBuy { get; private set; }


        private string _nameOfShop;
        private string _address;
        private float _id;
        public Dictionary<string, ProductParam> AmountOfGoods { get; private set; }

        private Shop()
        {
        }

        public void DecreaseMoney(float money)
        {
            this.Money -= money;
        }

        public void SetProductsToBuy(Product[] products)
        {
            ProductsToBuy = new List<Product>(products);
        }

        public Shop(string nameOfShop = "Ilia's shop", string address = "Street 5", float id = 25,
            float startCapital = 500f)
        {
            Money = startCapital;
            this._nameOfShop = nameOfShop;
            this._address = address;
            this._id = id;
            AmountOfGoods = new Dictionary<string, ProductParam>();
        }

        public int GetAmountOfGoodWithThisName(NameOfProduct name)
        {
            int amount = AmountOfGoods[name].amount;
            return amount;
        }

        public void SetPriceForGoodsWithName(NameOfProduct name, float price)
        {
            if (!AmountOfGoods.ContainsKey(name)) throw new Exception("There is no such good");
            AmountOfGoods[name].ChangePrice(price);
        }

        public void AddGoods(params Product[] newGoods)
        {
            foreach (Product product in newGoods)
            {
                var name = product.Name;
                var amount = product.Param.amount;
                var price = product.Param.price;
                if (AmountOfGoods.ContainsKey(name))
                {
                    if (amount > 0) AmountOfGoods[name].AddAmount(amount);
                    AmountOfGoods[name].ChangePrice(price);
                }
                else
                {
                    var param = new ProductParam(price, amount);
                    AmountOfGoods.Add(name, param);
                }
            }
        }

        public Product GetGood(NameOfProduct nameOfGood)
        {
            if (!AmountOfGoods.ContainsKey(nameOfGood)) return null;
            var param = AmountOfGoods[nameOfGood];
            Product good = new Product(nameOfGood, param);
            return good;
        }

        public void ServeGood(ICanBuy customer)
        {
            float totalSum = customer.ProductsToBuy.Sum(t => t.Param.price * t.Param.amount);
            var goods = customer.ProductsToBuy;
            if (totalSum > customer.Money) throw new Exception("Not enough money for purchase");
            foreach (var good in goods)
            {
                var name = good.Name;
                var amountOfGood = good.Param.amount;
                if (AmountOfGoods[name].amount < amountOfGood)
                    throw new Exception("Not enough good in shop for buying");
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
                var amount = AmountOfGoods[product.Name].amount;
                var price = AmountOfGoods[product.Name].price;
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
}