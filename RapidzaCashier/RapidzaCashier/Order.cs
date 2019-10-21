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

        private string _table;
        public string Table {
            get
            {
                return _table;
            }
            set
            {
                _table = value;
                NotifyChange("Table");
            }
        }

        public double TotalPrice => products.Sum(orderItem => orderItem.Value * orderItem.Key.Price);

        public Order()
        {
            products = new Dictionary<Product, int>();
        }

    public void Add(Product product, int count = 1)
        {
            if (products.ContainsKey(product))
            {
                products[product] += count;
            }
            else
            {
                products[product] = count;
            }
            NotifyChange("TotalPrice");
        }

        public void Remove(Product product)
        {
            products.Remove(product);
            NotifyChange("TotalPrice");

        }

        public void Clear()
        {
            products.Clear();
            NotifyChange("TotalPrice");
        }

        #region INotifyPropertyChanged Members and Methods
        private void NotifyChange(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


    }
}
