namespace Shops
{
    public class NameOfProduct
    {
        public string Name { get; private set; }

        public NameOfProduct(string name)
        {
            Name = name.ToLower();
        }

        public static implicit operator NameOfProduct(string name)
        {
            return new NameOfProduct(name);
        }

        public static implicit operator string(NameOfProduct num)
        {
            return num.Name;
        }
    }
}