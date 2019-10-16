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

        public WaitingProduct(Product product, string table, bool isReady = false ):base(product)
        {
            Table = table;
            IsReady = isReady;
        }

        public WaitingProduct(WaitingProduct waitingProduct)
            :base(waitingProduct.Name, waitingProduct.Image,waitingProduct.Price)
        {
            Table = waitingProduct.Table;
            IsReady = waitingProduct.IsReady;
        }
    }
}
