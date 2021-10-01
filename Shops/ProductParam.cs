using System;

namespace Shops
{
    public class ProductParam
    {
        public float price { get; private set; }


        public int amount { get; private set; }

        public ProductParam(float price, int amount)
        {
            this.price = price;
            this.amount = amount;
        }

        public void ChangePrice(float price)
        {
            this.price = price;
        }

        public void ChangeAmount(int amount)
        {
            if (amount < 0) throw new Exception("Negative amount of Good");
            this.amount = amount;
        }

        public void AddAmount(int amount)
        {
            this.amount += amount;
        }

        public void DecreaseAmount(int amount)
        {
            this.amount -= amount;
        }
    }
}