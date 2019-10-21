using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RapidzaCashier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IList<Product> AvailableProducts;
        private Order order;
        private ObservableWaitingProductsCollection WaitingProducts;

        const string PRODUCTS_FILE = "data/products.json";

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string productsAsJson = File.ReadAllText(PRODUCTS_FILE);
                AvailableProducts = JsonConvert.DeserializeObject<List<Product>>(productsAsJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }


            lwProductsList.ItemsSource = AvailableProducts;
            ICollectionView view = CollectionViewSource.GetDefaultView(AvailableProducts);
            view.Filter = this.FilterProductByTextBoxInput;

            order = new Order();
            lbProductsOrdered.ItemsSource = order.products;
            lblTotalPrice.DataContext = order;
            tbTable.DataContext = order;
            WaitingProducts = new ObservableWaitingProductsCollection();

            tabReadyOrders.DataContext = WaitingProducts;
            lwWaitingProducts.ItemsSource = WaitingProducts;


        }

        private bool FilterProductByTextBoxInput(object product)
        {
            bool isTextBoxEmpty = string.IsNullOrEmpty(tbSearchProduct.Text);
            bool productContainsSearchwords = (product as Product).Name.ToLower().Contains(tbSearchProduct.Text.ToLower());
            return isTextBoxEmpty || productContainsSearchwords;
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listViewItem = (ListViewItem)sender;
            var selectedProduct = (Product)listViewItem.DataContext;
          
            order.Add(selectedProduct);

            //TODO Find a way to bind data without needing to refresh manually
            lbProductsOrdered.Items.Refresh();
            

        }

        private void RemoveProductFromOrder(object sender, RoutedEventArgs e)
        {
            KeyValuePair<Product, int> data = (KeyValuePair<Product, int>)(sender as Button).DataContext;
            order.Remove(data.Key);
            lbProductsOrdered.Items.Refresh();
        }

        private void BtnSubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            foreach (var product in order.products)
                WaitingProducts.AddRepeatdly(new WaitingProduct(product.Key, order.Table), times: product.Value);
            
            order.Clear();
            lbProductsOrdered.Items.Refresh();
            order.Table = "";
        }

        private void TbSearchProduct_KeyUp(object sender, KeyEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lwProductsList.ItemsSource).Refresh();

        }

       

        private void DelmeDebugButton_Click(object sender, RoutedEventArgs e)
        {
            WaitingProducts.MarkFirstWaitingProductAsReady();
          
        }

        private void BtnProductDelivered_Click(object sender, RoutedEventArgs e)
        {
            var product = (WaitingProduct)(sender as Button).DataContext;
            WaitingProducts.Remove(product);
        }

        private void BtnSearchClear_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
