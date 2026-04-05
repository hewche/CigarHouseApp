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
using System.Windows.Shapes;
using static CigarHouseApp.Helpers.ProductFilter;

namespace CigarHouseApp.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public User currentUser = new User() { CartNavigation = new Usercart(), FavoritesNavigation = new Userfavorite()};
        
        public static Frame frame;

        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(User user)
        {

            currentUser = user;
            InitializeComponent();        
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tc && tc.IsLoaded)
            {
                switch (tabControlProducts.SelectedIndex)
                {
                    case 0:
                        cigarFrame.Navigate(new Pages.ListProductsPage(ProductStatus.CIGAR));
                        break;
                    case 1:
                        cigarFrame.Navigate(new Pages.ListProductsPage(ProductStatus.ACCESSORY));
                        break;
                    case 2:
                        cigarFrame.Navigate(new Pages.FavoritesPage(currentUser.FavoritesNavigation.Products.ToList()));
                        break;
                }
            }
        }

        private void tabControlProducts_Selected(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void tabControlProducts_Loaded(object sender, RoutedEventArgs e)
        {
            tabControlProducts.SelectedIndex = -1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cigarFrame.Navigate(new Pages.CartPage());
        }
    }
}
