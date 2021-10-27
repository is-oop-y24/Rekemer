using System.Collections.Generic;

namespace Shops
{
    public interface ICanBuy
    {
        public float Money { get; }
        public List<Product> ProductsToBuy { get; }
        public void DecreaseMoney(float money);
    }
}