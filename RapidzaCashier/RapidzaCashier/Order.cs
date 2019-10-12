using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidzaCashier
{
    class Order
    {
        /// <summary>
        /// Dictionarry that maps every product present in the order 
        /// to the ammount of portions ordered 
        /// </summary>
        private IDictionary<Product, int> products;

        public double TotalPrice => products.Sum(orderItem => orderItem.Value * orderItem.Key.Price);

        public Order()
        {
            products = new Dictionary<Product, int>();
        }

        public void Add(Product product, int count)
        {
            if (products.ContainsKey(product))
            {
                products[product] += count;
            }
            else
            {
                products[product] = count;
            }
        }

        public bool Remove(Product product)
        {
            return products.Remove(product);
        }
    }
}
