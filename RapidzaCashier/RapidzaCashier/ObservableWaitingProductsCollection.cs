using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidzaCashier
{
    class ObservableWaitingProductsCollection : ObservableCollection<WaitingProduct>
    {
        public int ReadyProductsCount => base.Items.Count(product => product.IsReady);

        public void AddRepeatdly(WaitingProduct product, int times)
        {
            for (int i = 0; i < times; i++)
            {
                base.Add(new WaitingProduct(product));
            }
        }

        public int MarkFirstWaitingProductAsReady(int index = 0)
        {
            if (index >=base.Count)
                return -1;

            if (!this.IsReady(index))
            {
                this.MarkReady(index);
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
            base.OnPropertyChanged(new PropertyChangedEventArgs("ReadyProductsCount"));
            base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool IsReady(int index)
        {
            return Items[index].IsReady;
        }

    }
}
