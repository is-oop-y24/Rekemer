using System.Collections.Generic;

namespace Shops
{
    public class ShopBuilder
    {
        private string _nameOfShop;
        private string _address;
        public float Money { get; private set; }
        public List<Product> ProductsToBuy { get; private set; }

        public static implicit operator Shop(ShopBuilder builder)
        {
            return builder.Build();
        }

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

        public ShopBuilder ToBuild()
        {
            ShopBuilder shopBuilder = new ShopBuilder();
            return shopBuilder.WithName(_nameOfShop).WithMoney(Money).WithAddress(_address);
        }
    }
}