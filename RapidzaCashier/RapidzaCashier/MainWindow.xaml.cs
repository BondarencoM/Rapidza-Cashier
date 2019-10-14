using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private IList<WaitingProduct> WaitingProducts;

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

            order = new Order();
            lbProductsOrdered.ItemsSource = order.products;
            lblTotalPrice.DataContext = order;
            tbTable.DataContext = order;
            WaitingProducts = new ObservableCollection<WaitingProduct>();

            tabReadyOrders.DataContext = WaitingProducts;
            lwWaitingProducts.ItemsSource = WaitingProducts;
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (ListViewItem)sender;
            var data = (Product)item.DataContext;
            if (data.Name.Equals(tbSearchProduct.Text))
            {
                BtnIncrementQuantity_Click(null, null);
            }
            else
            {
                tbSearchProduct.Text = data.Name;
                tbProductQuantity.Text = "1";
            }
        }

        private void BtnDecrementQuantity_Click(object sender, RoutedEventArgs e)
        {
            var quantity = Convert.ToInt32(tbProductQuantity.Text);
            quantity--;
            tbProductQuantity.Text = quantity.ToString();
        }

        private void BtnIncrementQuantity_Click(object sender, RoutedEventArgs e)
        {
            var quantity = Convert.ToInt32(tbProductQuantity.Text);
            quantity++;
            tbProductQuantity.Text = quantity.ToString();
        }

        private void BtnAddProductToOrder_Click(object sender, RoutedEventArgs e)
        {
            if (lwProductsList.SelectedItem == null)
                return;
            var selectedProduct = (Product)lwProductsList.SelectedItem;
            int amount = Convert.ToInt32(tbProductQuantity.Text);
            order.Add(selectedProduct, amount);
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
            foreach (var item in order.products)
                for (int i = 0; i < item.Value; i++)
                    WaitingProducts.Add(new WaitingProduct(item.Key, order.Table));
            
            order.products.Clear();
            lbProductsOrdered.Items.Refresh();
            order.Table = "";
        }
    }
}
