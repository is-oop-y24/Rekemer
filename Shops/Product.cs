namespace Shops
{
    public class Product
    {
        public ProductParam Param;
        
        public string Name { get; private set; }
        
        public void ChangeName(NameOfProduct name)
        {
            this.Name = name;
        } 
        public Product(string nameOfGood, float price,int amount = 1)
        {
            Name = nameOfGood;
            Param = new ProductParam(price, amount);
        }

        public Product(string nameOfGood, ProductParam param)
        {
            Name = nameOfGood;
            this.Param = new ProductParam(param.price,param.amount);
        }
       
        
       
    }
}