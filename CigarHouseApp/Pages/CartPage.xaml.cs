using CigarHouseApp.Models;
using CigarHouseApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CigarHouseApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        ObservableCollection<Product> cartProducts;
        decimal subTotal = 0;
        decimal deliveryTotal = 0;
        decimal total = 0;
        public CartPage(List<Product> products)
        {
            InitializeComponent();
            cartProducts = new ObservableCollection<Product>(products);
            LoadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LoadData()
        {
            itemsControlCart.ItemsSource = cartProducts;  
            UpdateCost();
        }

        private void UpdateCost()
        {
            tbItemsCount.Text = $"{cartProducts.Count.ToString()} товаров";
            CalculateCost();
            tbDelivery.Text = deliveryTotal.ToString() + " ₽";
            tbSubtotal.Text = subTotal.ToString() + " ₽";
            tbTotal.Text = total.ToString() + " ₽";
        }

        private void CalculateCost()
        {
            subTotal = cartProducts.Sum(p => p.CostProduct*p.PurchaseAmount);
            if (cartProducts.Count > 2)
                deliveryTotal = 125;
            else
                deliveryTotal = 0;
            total = subTotal + deliveryTotal;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateItemContext(FrameworkElement item)
        {
            var temp = item.DataContext;
            item.DataContext = null;
            item.DataContext = temp;
        }

        private void plusProductButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button.DataContext is Product product)
            {
                if (product.PurchaseAmount < 100)
                {
                    product.PurchaseAmount++;
                }
                UpdateCost();
                UpdateItemContext(button.Parent as FrameworkElement);
            }
        }

        private void minusProductButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button.DataContext is Product product)
            {
                if (product.PurchaseAmount > 1)
                {
                    product.PurchaseAmount--;
                }
                else
                {
                    DeleteProduct(product);
                }
                UpdateCost();
                UpdateItemContext(button.Parent as FrameworkElement);
            }
        }

        private void DeleteProduct(Product product)
        {
            if(product != null)
            {
                MainWindow main = Application.Current.MainWindow as MainWindow;
                cartProducts.Remove(product);
                main.currentUser.Usercart.Products.Remove(product);
            }
        }
    }
}
