using CigarHouseApp.Helpers;
using CigarHouseApp.Models;
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
    /// Логика взаимодействия для FavoritesPage.xaml
    /// </summary>
    public partial class FavoritesPage : Page
    {
        List<Product> favoriteProducts;
        HeartIconConverter heartIconConverter = new HeartIconConverter();
        public FavoritesPage(List<Product> products)
        {
            favoriteProducts = products;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(favoriteProducts != null)
            {
                favoriteItemsControl.ItemsSource = favoriteProducts;
            }
        }

        private void HeartButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            heartIconConverter.ToggleHeartColor(button);
        }
    }
}
