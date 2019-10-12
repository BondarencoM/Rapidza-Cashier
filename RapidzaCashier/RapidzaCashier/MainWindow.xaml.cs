using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();
            AvailableProducts = new List<Product>();
            AvailableProducts.Add(new Product("Rancho ",10));
            AvailableProducts.Add(new Product("Capriciosa ",12));
            AvailableProducts.Add(new Product("Diablo ",11));
            AvailableProducts.Add(new Product("Margaretta ",8));
            AvailableProducts.Add(new Product("Hawai ",9));
            AvailableProducts.Add(new Product("FOO ",9));
            AvailableProducts.Add(new Product("BAR ",9));
            AvailableProducts.Add(new Product("BUZZ ",9));  
            AvailableProducts.Add(new Product("FLUPKE ",9));
            AvailableProducts.Add(new Product("FLUPKE ",9));
            AvailableProducts.Add(new Product("FLUPKE ",9));
            AvailableProducts.Add(new Product("FLUPKE ",9));
            lwProductsList.ItemsSource = AvailableProducts;

            order = new Order();
            order.Add(AvailableProducts[0], 1);
            order.Add(AvailableProducts[1], 2);
            order.Add(AvailableProducts[4], 1);
            lbProductsOrdered.ItemsSource = order.products;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Product p = new Product("Rancho", "", 4);
            Order order = new Order();
            order.Add(p, 10);
            Console.WriteLine(order.TotalPrice);
            order.Add(p, 5);
            Console.WriteLine(order.TotalPrice);

        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
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
            Console.WriteLine("dd");
        }
    }
}
