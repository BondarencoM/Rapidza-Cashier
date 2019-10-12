using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidzaCashier
{
    class OrderProduct
    {
        public Product Product { get; }
        public int Count { get; set; }

        public OrderProduct(Product product)
        {
            this.Product = product;
        }

        public OrderProduct(Product product, int count) :this(product)
        {
            this.Count = count;
        }
    }
}
