using System;

namespace Shops
{
    public class Product
    {
        public Product(string nameOfGood, float price, int amount = 1)
        {
            Name = nameOfGood;
            this.Amount = amount;
            this.Price = price;
        }

        public Product(Product product)
        {
            this.Price = product.Price;
            this.Amount = product.Amount;
            Name = product.Name;
        }

        public float Price { get; private set; }
        public int Amount { get; private set; }
        public string Name { get; private set; }

        public void ChangeName(string name)
        {
            this.Name = name;
        }

        public void ChangePrice(float price)
        {
            this.Price = price;
        }

        public void ChangeAmount(int amount)
        {
            if (amount < 0) throw new Exception("Negative amount of Good");
            this.Amount = amount;
        }

        public void AddAmount(int amount)
        {
            this.Amount += amount;
        }

        public void DecreaseAmount(int amount)
        {
            this.Amount -= amount;
        }
    }
}