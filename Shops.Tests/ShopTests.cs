using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using NUnit.Framework;


namespace Shops.Tests
{
    public class ShopTests
    {
        private ShopBuilder _shopBuilder = new ShopBuilder();
        private ProductBuilder _productBuilder = new ProductBuilder();

        [SetUp]
        public void Setup()
        {
            _shopBuilder = new ShopBuilder();
            _productBuilder = new ProductBuilder();
        }
        
        [Test]
        public void AddGoods_GoodsAreAdded()
        {
            Shop shop = _shopBuilder.WithName("Ilia's shop");
            Product apple = _productBuilder.WithName("apple").WithAmount(7);
            Product[] goods = new Product[] {apple};
            shop.AddGoods(goods);
            Product lemon = _productBuilder.WithName("lemon").WithAmount(2);
            Product[] goods1 = new Product[] {lemon};
            shop.AddGoods(goods1);
            var amountOfgoods = shop.AmountOfGoods.Count;
            Assert.Greater(amountOfgoods, 0);
        }
        [Test]
        public void AddGoods_GoodsCanBeGot()
        {
            Shop shop = _shopBuilder.WithName("Ilia's shop");
            Product lemon = _productBuilder.WithName("lemon").WithAmount(4);
            Product[] goods = new Product[] {lemon};
            shop.AddGoods(goods);
            Assert.AreEqual("lemon", shop.GetGood("lemon").Name);
        }

        [Test]
        public void ChangePriceForGoodWithName_PriceIsChanged()
        {
            Shop shop = _shopBuilder.WithName("Ilia's shop");
            Product apple = _productBuilder.WithName("apple").WithAmount(7).WithPrice(3f);
            Product[] goods = new Product[] {apple};
            shop.AddGoods(goods);
            shop.SetPriceForGoodsWithName("apple", 10f);
            Assert.AreEqual(10f, shop.GetGood("apple").Price);
        }

        [Test]
        public void AddProducts_AmountIsCorrect()
        {
            Shop shop = _shopBuilder.WithName("Ilia's shop");
            Product apple = _productBuilder.WithName("apple").WithAmount(5);
            Product[] goods = new Product[] {apple};
            shop.AddGoods(goods);
            Assert.AreEqual(5, shop.GetAmountOfGoodWithThisName("apple"));
        }

        [Test]
        public void ProductsBought_StateIsChanged()
        {
            Shop shop = _shopBuilder.WithName("Ilia's shop");
            Product apple = _productBuilder.WithName("apple").WithAmount(5);
            Product apple0 = _productBuilder.WithName("apple").WithAmount(8);
            Product[] goods = new Product[] {apple};
            Product[] goodsOfShop = new Product[] {apple0};
            Customer customer = new Customer(500f, goods);
            shop.AddGoods(goodsOfShop);
            shop.ServeGood(customer);
            Assert.AreEqual(3, shop.GetAmountOfGoodWithThisName("apple"));
        }

        [Test]
        
        private TestDelegate TryToBuy()
        {
            Shop shop = _shopBuilder.WithName("Ilia's shop");
            Product apple = _productBuilder.WithName("apple").WithAmount(5);
            Product appleOfShop = _productBuilder.WithName("apple").WithAmount(8).WithPrice(20f);
            Product[] goodsToBuy = new Product[] {apple};
            var goodsOfShop = new Product[] {appleOfShop};
            var customer = new Customer(10f, goodsToBuy);
            shop.AddGoods(appleOfShop);
            shop.ServeGood(customer);
            return null;
        }
        
        [Test]
        public void CustomerDoesntHaveEnoughMoney_ThrowException()
        {
            Assert.Throws<Exception>(delegate { TryToBuy();});
        }

    

        [Test]
        public void CustomerBoughtGoods_ShopGotMoney()
        {
            Shop shop = _shopBuilder.WithName("Ilia's shop").WithMoney(500f);
            Product apple = _productBuilder.WithName("apple").WithAmount(5);
            Product apple0 = _productBuilder.WithName("apple").WithAmount(8).WithPrice(3f);
            Product[] goods = new Product[] {apple};
            Product[] goodsOfShop = new Product[] {apple0};
            Customer customer = new Customer(500f, goods);
            shop.AddGoods(goodsOfShop);
            shop.ServeGood(customer);
            Assert.AreEqual(515, shop.Money);
        }

        [Test]
        public void FewShops_FoundCheapestPrice()
        {
            Shop shop = _shopBuilder.WithName("Ilia's shop").WithMoney(1000f);
            Shop notExpensiveShop = _shopBuilder.WithName("Fane's Shop").WithMoney(101f).WithAddress("Street 22");
            Shop expensiveShop = _shopBuilder.WithName("Shepard'sShop").WithMoney(342).WithAddress("Street 11");
            Product apple = _productBuilder.WithName("apple").WithAmount(5).WithPrice(10f);
            Product lessExpensinveApple = _productBuilder.WithName("apple").WithAmount(8).WithPrice(50f);
            Product expensinveApple = _productBuilder.WithName("apple").WithAmount(8).WithPrice(100f);
            Product[] goodsToBuy = new Product[] {apple};
            Product[] goodsOfShop = new Product[] {lessExpensinveApple};
            Product[] goodsOfMoreExpensiveShop = new Product[] {expensinveApple};
            shop.SetProductsToBuy(goodsToBuy);
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
            Product apple = _productBuilder.WithName("apple").WithAmount(4).WithPrice(2f);
            Product appleOfImporter = _productBuilder.WithName("apple").WithAmount(8).WithPrice(24f);
            Product[] goodsToBuy = new Product[] {apple};
            Product[] goodsOfImporter = new Product[] {appleOfImporter};
            Shop shop = new Shop("Ilia's Shop", "Street 5", 1000f);
            shop.SetProductsToBuy(goodsToBuy);
            Shop importer = new Shop("Fane's Shop", "Street 22", 101);
            importer.AddGoods(goodsOfImporter);
            importer.ServeGood(shop);
            shop.UpdateGoods();
            Assert.AreEqual(4, importer.GetGood("apple").Amount);
            Assert.AreEqual(4, shop.GetGood("apple").Amount);
        }
    }
}