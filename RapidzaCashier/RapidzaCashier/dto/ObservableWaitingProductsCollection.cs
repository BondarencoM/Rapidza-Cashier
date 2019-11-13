using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidzaCashier.dto
{
    class ObservableWaitingProductsCollection : ObservableCollection<WaitingProduct>
    {
        public int ReadyProductsCount => Items.Count(product => product.IsReady);

        public void AddRepeatdly(WaitingProduct product, int times)
        {
            for (int i = 0; i < times; i++)
            {
                Add(new WaitingProduct(product));
            }
        }

        public int MarkFirstWaitingProductAsReady(int index = 0)
        {
            if (index >= Count)
                return -1;

            if (!IsReady(index))
            {
                MarkReady(index);
                return index;
            }
            else
            {
                return MarkFirstWaitingProductAsReady(index + 1);
            }
        }

        public void MarkReady(int index)
        {
            Items[index].IsReady = true;
            NotifyPropertAndCollectionChanged("ReadyProductsCount");
        }

        public bool IsReady(int index)
        {
            return Items[index].IsReady;
        }

        public new void Remove(WaitingProduct product)
        {
            base.Remove(product);
            NotifyPropertAndCollectionChanged("ReadyProductsCount");
        }

        private void NotifyPropertAndCollectionChanged(string property)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs(property));
            base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

    }
}
