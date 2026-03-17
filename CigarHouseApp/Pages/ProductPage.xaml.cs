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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
        }

        public ProductPage(Product product)
        {
            InitializeComponent();
            LoadDataProduct(product);   
        }

        private void likeButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LoadDataProduct(Product product)
        {
            svStatsCigar.DataContext = product;
            spProductInfo.DataContext = product;
            tbBrandName.DataContext = product;
        }

    }
}
