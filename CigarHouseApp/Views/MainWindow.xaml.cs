using CigarHouseApp.Helpers;
using CigarHouseApp.Models;
using ControlzEx.Standard;
using Microsoft.EntityFrameworkCore;
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
using static CigarHouseApp.Helpers.NavigateService;
using static CigarHouseApp.Helpers.ProductFilter;

namespace CigarHouseApp.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public User currentUser = new User() {Login="unknown", Usercart = new Usercart(), Userfavorite = new Userfavorite()};
        
        public static Frame frame;
        public PageType previousPage;
        public PageType currentPage;

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
            tbUserName.DataContext = currentUser;
            ConfigureRoleTabs();
        }

        

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tc && tc.IsLoaded)
            {
                if (tabControlProducts.SelectedItem == tabCigars)
                {
                    cigarFrame.Navigate(new Pages.ListProductsPage(ProductStatus.CIGAR));
                    currentPage = PageType.ListProductPage;
                }
                else if (tabControlProducts.SelectedItem == tabAccessories)
                {
                    cigarFrame.Navigate(new Pages.ListProductsPage(ProductStatus.ACCESSORY));
                    currentPage = PageType.ListProductPageAccessories;
                }
                else if (tabControlProducts.SelectedItem == tabFavorites)
                {
                    cigarFrame.Navigate(new Pages.FavoritesPage(currentUser.Userfavorite.Products.ToList()));
                    currentPage = PageType.FavoritesPage;
                }
                else if (tabControlProducts.SelectedItem == tabMyOrders)
                {
                    cigarFrame.Navigate(new Pages.OrdersPage(currentUser));
                    currentPage = PageType.BestProductsPage;
                }
                else if (tabControlProducts.SelectedItem == tabAddProduct)
                {
                    cigarFrame.Navigate(new Pages.AddProductPage());
                }
                else if (tabControlProducts.SelectedItem == tabOrderManagement)
                {
                    cigarFrame.Navigate(new Pages.OrderManagmentPage());
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

        private void ConfigureRoleTabs()
        {
            bool hasManagementAccess = currentUser?.RoleId == 1 || currentUser?.RoleId == 3;
            tabAddProduct.Visibility = hasManagementAccess ? Visibility.Visible : Visibility.Collapsed;
            tabOrderManagement.Visibility = hasManagementAccess ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cigarFrame.Navigate(new Pages.CartPage(currentUser.Usercart.Products.ToList()));
        }


        private void btnUserProfile_Click(object sender, RoutedEventArgs e)
        {
            
            UserUpdateProfileWindow userUpdateProfileWindow = new UserUpdateProfileWindow(currentUser);
            if(userUpdateProfileWindow.ShowDialog() == true )
            {
                UpdateUser(currentUser.UserId);
            }
            else
            {

            }
        }

        private void UpdateUser(int userId)
        {
            using (var context = new CigarhouseContext())
            {
                currentUser =  context.Users
                .Include(u => u.Usercart)
                    .ThenInclude(uc => uc.Products)
                .Include(u => u.Userfavorite)
                    .ThenInclude(uf => uf.Products)
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderStatus)
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(i => i.Product)
                .FirstOrDefault(u => u.UserId == userId);
            }
        }
    }
}
