using CigarHouseApp.Models;
using CigarHouseApp.Views;
using System;
using System.Collections.Generic;
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
        List<Product> cartProducts;
        public CartPage(List<Product> products)
        {
            InitializeComponent();
            cartProducts = products;
            LoadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LoadData()
        {
            itemsControlCart.ItemsSource = cartProducts;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
