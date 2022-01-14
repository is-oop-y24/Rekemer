using System;
using System.Collections.Generic;

namespace Shops
{
    public class Customer : ICanBuy
    {
        public Customer(float money, params Product[] newGoods)
        {
            this.Money = money;
            ProductsToBuy = new List<Product>(newGoods);
        }

        public float Money { get; private set; }
        public List<Product> ProductsToBuy { get; private set; }

        public void DecreaseMoney(float money)
        {
            if (this.Money < money) throw new Exception("Customer Doesnt have enough money");
            this.Money -= money;
        }

        public void AddProducts(Product newGood)
        {
        }
    }
}