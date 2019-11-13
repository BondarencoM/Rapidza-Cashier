using Newtonsoft.Json;
using RapidzaCashier.Connection;
using RapidzaCashier.dto;
using System;
using System.Collections;
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
        private Order currentOrder;
        private ObservableWaitingProductsCollection WaitingProducts;

        DataSender cookSender;
        DataReceiver arduinoReceiver;

        private const string PRODUCTS_FILE = "data/products.json";

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Window_Loaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TryToReadAvailableProducts();
            SetFiltertoCollectionView(AvailableProducts, this.FilterProductByTextBoxInput);
            SetItemSources();
            SetUpConnection();
        }

        private void TryToReadAvailableProducts()
        {
            try
            {
                ReadAvailableProducts();
            }
            catch (Exception ex)
            {
                DisplayErrorAndExit(ex);
            }
        }

        private void ReadAvailableProducts()
        {
            string productsAsJson = File.ReadAllText(PRODUCTS_FILE);
            AvailableProducts = JsonConvert.DeserializeObject<List<Product>>(productsAsJson);
        }

        private void DisplayErrorAndExit(Exception ex)
        {
            DisplayError(ex.Message);
            Application.Current.Shutdown();
        }

        private void DisplayError(string message)
        {
            MessageBox.Show("Rapidza Cashier Error: " + message);
        }

        private void SetFiltertoCollectionView(IEnumerable target, Predicate<object> filterFunction)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(target);
            view.Filter = filterFunction;
        }

        private bool FilterProductByTextBoxInput(object product)
        {
            return isSearchTextBoxEmpty() || productContainsSearchwords(product as Product);
        }

        private bool isSearchTextBoxEmpty()
        {
            return string.IsNullOrEmpty(tbSearchProduct.Text);
        }

        private bool productContainsSearchwords(Product product)
        {
            string productName = product.Name.ToLower();
            string searchSequence = tbSearchProduct.Text.ToLower();
            return productName.Contains(searchSequence);
        }

        private void SetItemSources()
        {
            SetAvailableProductsItemSource();
            SetOrderItemSource();
            SetWaitingProductsItemSource();
        }

        private void SetAvailableProductsItemSource()
        {
            lwProductsList.ItemsSource = AvailableProducts;
        }

        private void SetOrderItemSource()
        {
            currentOrder = new Order();
            lbProductsOrdered.DataContext = currentOrder;

            lblTotalPrice.DataContext = currentOrder;
            tbTable.DataContext = currentOrder;
        }
        private void SetWaitingProductsItemSource()
        {
            WaitingProducts = new ObservableWaitingProductsCollection();
            tabReadyOrders.DataContext = WaitingProducts;
            lwWaitingProducts.ItemsSource = WaitingProducts;
        }

        private void SetUpConnection()
        {
            SetUpDefaults();
        }

        private void SetUpDefaults()
        {
            lblThisMachineIp.Text = WebConnection.GetLocalIP();
            //tbArduinoIP.Text = "192.168.178.57";
            tbArduinoPort.Text = "8081";

            tbCookIP.Text = "192.168.0.102";
            tbCookPort.Text = "8082";

            arduinoReceiver = new DataReceiver(int.Parse(tbArduinoPort.Text));
            arduinoReceiver.messageReceived += ReceivedDataFromArduino;

            cookSender = new DataSender(tbCookIP.Text, int.Parse(tbCookPort.Text));
           
        }

        #endregion

        #region Events

        private void ReceivedDataFromArduino(object sender, string message)
        {
            if (message.ToLower() == "product_ready")
            {
                WaitingProducts.MarkFirstWaitingProductAsReady();
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listViewItem = (ListViewItem)sender;
            var selectedProduct = (Product)listViewItem.DataContext;

            currentOrder.Add(selectedProduct);
        }

        private void RemoveProductFromOrder(object sender, RoutedEventArgs e)
        {
            var butonDataContext = (sender as Button).DataContext;
            var data = (KeyValuePair<Product, int>)butonDataContext;
            currentOrder.Remove(data.Key);
        }

        private void BtnSubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            var orderedProducts = new List<OrderedProductDto>();

            foreach (var product in currentOrder.Products)
            {
                var waitingProduct = new WaitingProduct(product.Key, currentOrder.Table);
                WaitingProducts.AddRepeatdly(waitingProduct, times: product.Value);

                AddItemRepeatedly(orderedProducts, product);
            }

            SendOrdersToCook(orderedProducts);

            currentOrder.Clear();
            currentOrder.Table = "";
        }


        private static void AddItemRepeatedly(List<OrderedProductDto> orderedProducts, KeyValuePair<Product, int> products)
        {
            for (int i = 0; i < products.Value; i++)
            {
                var orderedProductDto = new OrderedProductDto(products.Key.Name, products.Key.Image);
                orderedProducts.Add(orderedProductDto);
            }
        }

        private void SendOrdersToCook(List<OrderedProductDto> orderedProducts)
        {
            string message = JsonConvert.SerializeObject(orderedProducts);
            cookSender.sendMessage(message);
        }

        private void TbSearchProduct_KeyUp(object sender, KeyEventArgs e)
        {
            var collectionView = CollectionViewSource.GetDefaultView(lwProductsList.ItemsSource);
            collectionView.Refresh();
        }

        private void BtnProductDelivered_Click(object sender, RoutedEventArgs e)
        {
            var product = (WaitingProduct)(sender as Button).DataContext;
            WaitingProducts.Remove(product);
        }

        private void BtnSearchClear_Click(object sender, RoutedEventArgs e)
        {
            tbSearchProduct.Clear();

            //Manually Trigger search filtering to show all prodcuts
            TbSearchProduct_KeyUp(null, null);
        }

        private void btnReestablishArduinoConnection_Click(object sender, RoutedEventArgs e)
        {
            arduinoReceiver.close();
            arduinoReceiver = new DataReceiver(int.Parse(tbArduinoPort.Text));

        }

        private void btnTestCookConnection_Click(object sender, RoutedEventArgs e)
        {
            cookSender.sendMessage("ping");

        }
        private void tbCookPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool connectionDetailsEmpty = string.IsNullOrEmpty(tbCookPort.Text + tbCookIP.Text);
            bool isSenderNotSetUp = cookSender == null;

            if (connectionDetailsEmpty || isSenderNotSetUp)
                return;

            cookSender.setDestination(tbCookIP.Text, int.Parse(tbCookPort.Text));
        }

        #endregion

    }
}
