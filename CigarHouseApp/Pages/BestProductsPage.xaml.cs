using CigarHouseApp.Helpers;
using CigarHouseApp.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CigarHouseApp.Helpers.NavigateService;
using static CigarHouseApp.Helpers.ProductFilter;

namespace CigarHouseApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для BestProductPage.xaml
    /// </summary>
    public partial class BestProductsPage : Page
    {
        CartFavoritesService cartFavoritesService = new CartFavoritesService();
        List<Product> products = new List<Product>();
        public BestProductsPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await Page_LoadedAsync();
            listViewProducts.ItemsSource = products;
        }


        private async Task Page_LoadedAsync()
        {
            try
            {
                await LoadDataAsync();
                loadingGrid.Visibility = Visibility.Collapsed;
                listViewProducts.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                loadingGrid.Visibility = Visibility.Collapsed;
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }
        private async Task LoadDataAsync()
        {
            await Dispatcher.InvokeAsync(() => loadingGrid.Visibility = Visibility.Visible);

            var bestProducts = await Task.Run(() =>
            {
                using (var _context = new CigarhouseContext())
                {
                    List<Product> loadedProducts;

                    var bestProductIds = _context.BestProducts.Select(b => b.ProductId).ToHashSet();

                    loadedProducts = _context.Products.Where(p => bestProductIds.Contains(p.ProductId))
                        .Include(p => p.Brand)
                        .Include(p => p.Cigar)
                        .Include(p => p.Accessory)
                        .Include(p => p.CountryNavigation)
                        .ToList();

                    return cartFavoritesService.SetOptions(loadedProducts);
                }
            });


            await Task.Delay(100);

            await Dispatcher.InvokeAsync(() =>
            {
                products = bestProducts;
                listViewProducts.ItemsSource = products;


                loadingGrid.Visibility = Visibility.Collapsed;
                listViewProducts.Visibility = Visibility.Visible;
            });
        }
        private void tbBuyButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            Button button = sender as Button;
            if (button.DataContext is Product product)
            {
                main.currentUser.Usercart.Products.Add(product);
            }
        }

        private void HeartButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            //heartIconConverter.ToggleHeartColor(button);

            if (button.DataContext is Product product)
            {

                cartFavoritesService.ToggleFavorites(product);
                //var temp = button.DataContext;
                //button.DataContext = null;
                //button.DataContext = temp;
                UpdateItemContext(button);
            }
        }

        private void tbProductName_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;
            if (hyperlink.DataContext is Product product)
            {
                listViewProducts.SelectedItem = product;
                MainWindow main = Application.Current.MainWindow as MainWindow;
                main.cigarFrame.Navigate(new Pages.ProductPage(listViewProducts.SelectedItem as Product, product.Cigar == null? ProductStatus.ACCESSORY : ProductStatus.CIGAR, PageType.ListProductPage));
            }
        }


        private void UpdateItemContext(FrameworkElement item)
        {
            var temp = item.DataContext;
            item.DataContext = null;
            item.DataContext = temp;
        }
        private void tbAddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button.DataContext is Product product)
            {
                cartFavoritesService.TogglePurchase(product);
                UpdateItemContext(button);
            }
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.DataContext is Product product)
            {
                listViewProducts.SelectedItem = product;
                MainWindow main = Application.Current.MainWindow as MainWindow;
                main.cigarFrame.Navigate(new Pages.ProductPage(listViewProducts.SelectedItem as Product, product.Cigar == null ? ProductStatus.ACCESSORY : ProductStatus.CIGAR, PageType.ListProductPage));
            }
        }

        private void minusButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button.DataContext is Product product)
            {
                if (product.PurchaseAmount > 0)
                {
                    product.PurchaseAmount--;
                }
                UpdateItemContext(button.Parent as FrameworkElement);
            }
        }

        private void plusButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button.DataContext is Product product)
            {
                if (product.PurchaseAmount < 100)
                {
                    product.PurchaseAmount++;
                }
                UpdateItemContext(button.Parent as FrameworkElement);
            }

        }
    }
}
