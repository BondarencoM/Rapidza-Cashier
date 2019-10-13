using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidzaCashier
{
    class Order : INotifyPropertyChanged
    {
        /// <summary>
        /// Dictionarry that maps every product present in the order 
        /// to the ammount of portions ordered 
        /// </summary>
        public IDictionary<Product, int> products;

        public double TotalPrice => products.Sum(orderItem => orderItem.Value * orderItem.Key.Price);

        public Order()
        {
            products = new Dictionary<Product, int>();
        }


        #region INotifyPropertyChanged Members and Methods
        private void NotifyChangePrice()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalPrice"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    

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
            NotifyChangePrice();
        }

        public void Remove(Product product)
        {
            products.Remove(product);
            NotifyChangePrice();

        }
    }
}
