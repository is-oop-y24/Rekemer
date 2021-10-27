using System.Linq;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using NUnit.Framework;


namespace Shops.Tests
{
    public class ShopTests
    {
        [Test]
        public void AddGoods_GoodsAreAdded()
        {
            Shop shop = new Shop("Ilia's Shop", "Street 5", 25);
            Product[] goods = new Product[]
                {new Product("apple", 2), new Product("banana", 5), new Product("apple", 4), new Product("apple", 1)};
            shop.AddGoods(goods);
            Product[] goods1 = new Product[]
            {
                new Product("lemon", 2), new Product("orange", 5), new Product("watermelon", 4), new Product("apple", 1)
            };
            shop.AddGoods(goods1);
            var amountOfgoods = shop.AmountOfGoods.Count;
            Assert.Greater(amountOfgoods, 0);
        }

        [Test]
        public void AddGoods_GoodsCanBeGot()
        {
            Shop shop = new Shop("Ilia's Shop", "Street 5", 25);
            Product[] goods = new Product[]
                {new Product("apple", 2), new Product("banana", 5), new Product("lemon", 4), new Product("apple", 1)};
            shop.AddGoods(goods);
            Assert.AreEqual("lemon", shop.GetGood("lemon").Name);
        }

        [Test]
        public void ChangePriceForGoodWithName_PriceIsChanged()
        {
            Shop shop = new Shop("Ilia's Shop", "Street 5", 25);
            Product[] goods = new Product[]
                {new Product("apple", 2), new Product("banana", 5), new Product("lemon", 4), new Product("apple", 1)};
            shop.AddGoods(goods);
            shop.SetPriceForGoodsWithName("apple", 10f);
            Assert.AreEqual(10f, shop.GetGood("apple").Param.price);
        }

        [Test]
        public void AddProducts_AmountIsCorrect()
        {
            Shop shop = new Shop("Ilia's Shop", "Street 5", 25);
            Product[] goods = new Product[]
            {
                new Product("aPple", 2, 4), new Product("banana", 5), new Product("lemon", 4), new Product("apple", 1)
            };
            shop.AddGoods(goods);
            Assert.AreEqual(5, shop.GetAmountOfGoodWithThisName("apple"));
        }

        [Test]
        public void ProductsBought_StateIsChanged()
        {
            Shop shop = new Shop("Ilia's Shop", "Street 5", 25);
            Product[] goods = new Product[]
            {
                new Product("apple", 2, 4), new Product("banana", 5), new Product("lemon", 4), new Product("apple", 1)
            };
            Product[] goodsOfShop = new Product[]
                {new Product("apple", 2, 8), new Product("banana", 6), new Product("lemon", 6)};
            Customer customer = new Customer(500f, goods);
            shop.AddGoods(goodsOfShop);
            shop.ServeGood(customer);
            Assert.AreEqual(3, shop.GetAmountOfGoodWithThisName("apple"));
        }

        [Test]
        public void CustomerDoesntHaveEnoughMoney_ThrowException()
        {
            Shop shop = new Shop("Ilia's Shop", "Street 5", 25);
            Product[] goods = new Product[]
            {
                new Product("apple", 2, 4), new Product("banana", 5), new Product("lemon", 4), new Product("apple", 1)
            };
            Product[] goodsOfShop = new Product[]
                {new Product("apple", 2, 8), new Product("banana", 6), new Product("lemon", 6)};
            Customer customer = new Customer(10f, goods);
            shop.AddGoods(goodsOfShop);
            shop.ServeGood(customer);
            Assert.AreEqual(3, shop.GetAmountOfGoodWithThisName("apple"));
        }

        [Test]
        public void CustomerBoughtGoods_ShopGotMoney()
        {
            Shop shop = new Shop("Ilia's Shop", "Street 5", 25); //default money 500
            Product[] goods = new Product[]
            {
                new Product("apple", 2, 4), new Product("banana", 5), new Product("lemon", 4), new Product("apple", 1)
            };
            Product[] goodsOfShop = new Product[]
                {new Product("apple", 2, 8), new Product("banana", 6), new Product("lemon", 6)};
            Customer customer = new Customer(500f, goods);
            shop.AddGoods(goodsOfShop);
            shop.ServeGood(customer);
            Assert.AreEqual(518, shop.Money);
        }

        [Test]
        public void FewShops_FoundCheapestPrice()
        {
            Product[] goodsToBuy = new Product[]
                {new Product("apple", 2, 4), new Product("banana", 5, 5), new Product("lemon", 4, 3)};
            Product[] goodsOfShop = new Product[]
                {new Product("apple", 2, 8), new Product("banana", 6), new Product("lemon", 6)};
            Product[] goodsOfMoreExpensiveShop = new Product[]
            {
                new Product("apple", 100, 4), new Product("banana", 100), new Product("lemon", 100),
                new Product("apple", 100)
            };
            Shop shop = new Shop("Ilia's Shop", "Street 5", 25, 1000f);
            shop.SetProductsToBuy(goodsToBuy);
            Shop notExpensiveShop = new Shop("Fane's Shop", "Street 22", 101);
            Shop expensiveShop = new Shop("Shepard'sShop", "Street 11", 342);
            notExpensiveShop.AddGoods(goodsOfShop);
            expensiveShop.AddGoods(goodsOfMoreExpensiveShop);
            var shops = new Shop[] {notExpensiveShop, expensiveShop};
            var cheapestShop = shop.FindCheapestCollectionOfGoodInShoops(shops);
            bool check = cheapestShop == notExpensiveShop;
            Assert.AreEqual(true, check);
        }

        [Test]
        public void ShopbuyFromAnotherShop_StatesAreChanged()
        {
            Product[] goodsToBuy = new Product[]
                {new Product("apple", 2, 4), new Product("banana", 5, 5), new Product("lemon", 4, 3)};
            // 45 
            Product[] goodsOfShop = new Product[]
                {new Product("apple", 2, 8), new Product("banana", 2, 6), new Product("lemon", 2, 4)};
            Product[] currentGoods = new Product[]
                {new Product("apple", 100, 5), new Product("banana", 100), new Product("lemon", 100)};
            Shop shop = new Shop("Ilia's Shop", "Street 5", 25, 1000f);
            shop.AddGoods(currentGoods);
            shop.SetProductsToBuy(goodsToBuy);
            Shop importer = new Shop("Fane's Shop", "Street 22", 101);
            importer.AddGoods(goodsOfShop);
            importer.ServeGood(shop);
            shop.UpdateGoods();
            //bool check = cheapestShop == importer;
            Assert.AreEqual(1000 - 45, shop.Money);
            Assert.AreEqual(9, shop.GetGood("apple").Param.amount);
            Assert.AreEqual(6, shop.GetGood("banana").Param.amount);
            Assert.AreEqual(4, shop.GetGood("lemon").Param.amount);
        }
    }
}