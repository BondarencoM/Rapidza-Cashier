using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidzaCashier
{
    class OrderProduct
    {
        public Product product { get; }
        public int count { get; set; }

        public OrderProduct(Product product)
        {
            this.product = product;
        }

        public OrderProduct(Product product, int count) :this(product)
        {
            this.count = count;
        }
    }
}
