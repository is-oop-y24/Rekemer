namespace Shops
{
    public class ProductBuilder
    {
        private float Price { get; set; }
        private int Amount { get;  set; }
        private string Name { get;  set; }

        public static implicit operator Product(ProductBuilder builder)
        {
            return builder.Build();
        }

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

        public ProductBuilder ToBuild()
        {
            ProductBuilder productBuilder = new ProductBuilder();
            return productBuilder.WithAmount(Amount).WithPrice(Price).WithName(Name);
        }
    }
}