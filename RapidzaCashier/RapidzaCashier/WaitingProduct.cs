using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidzaCashier
{
    class WaitingProduct : Product
    {
        public string Table { get; }
        public bool IsReady { get; set; }

        public WaitingProduct(Product product, string table, bool isReady = false ):base(product.Name, product.Image, product.Price)
        {
            Table = table;
            IsReady = isReady;
        }
    }
}
