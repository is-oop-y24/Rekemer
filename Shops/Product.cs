using System;


namespace Shops
{
    public class Product
    {
        public float Price { get; private set; }
        public int Amount { get; private set; }
        public string Name { get; private set; }

        public Product(Product product)
        {
            this.Price = product.Price;
            this.Amount = product.Amount;
            Name = product.Name;
        }

        public void ChangeName(string name)
        {
            this.Name = name;
        }

        public Product(string nameOfGood, float price, int amount = 1)
        {
            Name = nameOfGood;
            this.Amount = amount;
            this.Price = price;
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


    public class ProductBuilder
    {
        private float Price { get; set; }
        private int Amount { get;  set; }
        private string Name { get;  set; }

        public ProductBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ProductBuilder WithPrice(float price)
        {
            this.Price = price;
            return this;
        }

        public ProductBuilder WithAmount(int amount)
        {
            this.Amount = amount;
            return this;
        }

        public Product Build()
        {
            return new Product(Name, Price, Amount);
        }

        public static implicit operator Product(ProductBuilder builder)
        {
            return builder.Build();
        }

        public ProductBuilder ToBuild()
        {
            ProductBuilder productBuilder = new ProductBuilder();
            return productBuilder.WithAmount(Amount).WithPrice(Price).WithName(Name);
        }
    }
}