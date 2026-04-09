using CigarHouseApp.Helpers;
using CigarHouseApp.Models;
using CigarHouseApp.Views;
using ControlzEx.Standard;
using MaterialDesignColors;
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
using static CigarHouseApp.Helpers.ProductFilter;
using static CigarHouseApp.Views.MainWindow;

namespace CigarHouseApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListProductsPage.xaml
    /// </summary>
    public partial class ListProductsPage : Page
    {

        CartFavoritesService cartFavoritesService = new CartFavoritesService();
        List<Product> products = new List<Product>();
        List<Brand> brands = new List<Brand>();
        List<Country> countries = new List<Country>();

        

        Freezer searchFreezer;
        Freezer priceFreezer;
        Freezer categoryFreezer;

        ProductFilter productFilter;

        ProductStatus productStatus;

        private decimal minPrice = 0;
        private decimal maxPrice = 0;


        public ListProductsPage(ProductStatus status)
        {
            
            InitializeComponent();
            productStatus = status;
            Loaded += async (s, e) => await Page_LoadedAsync();
        }

        private async Task Page_LoadedAsync()
        {


            try
            {
                await LoadDataAsync();

                await LoadFiltersAsync();

                loadingGrid.Visibility = Visibility.Collapsed;
                cardFilters.Visibility = Visibility.Visible;
                listViewProducts.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                loadingGrid.Visibility = Visibility.Collapsed;
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }


        private void LoadService()
        {
            productFilter = new ProductFilter();
            searchFreezer = new Freezer(async ()=> {
            {
                    
                        listViewProducts.ItemsSource = await productFilter.ApplyFilters(products);
                }
            }, 300);
            priceFreezer = new Freezer(async ()=> listViewProducts.ItemsSource = await productFilter.ApplyFilters(products),300);
            categoryFreezer = new Freezer(async ()=> listViewProducts.ItemsSource = await productFilter.ApplyFilters(products),0);
        }



        private async Task LoadDataAsync()
        {
            await Dispatcher.InvokeAsync(() => loadingGrid.Visibility = Visibility.Visible);

            var productsFromDb = await Task.Run(() =>
            {
                using (var _context = new CigarhouseContext())
                {
                    List<Product> loadedProducts;

                    if (productStatus == ProductStatus.CIGAR)
                    {
                        loadedProducts = _context.Products
                            .AsNoTracking()
                            .Where(p => p.Accessory == null)
                            .Include(p => p.Brand)
                            .Include(p => p.Cigar)
                            .Include(p => p.CountryNavigation)
                            .ToList();
                    }
                    else
                    {
                        loadedProducts = _context.Products
                            .AsNoTracking()
                            .Where(p => p.Cigar == null)
                            .Include(p => p.Brand)
                            .Include(p => p.Accessory)
                            .Include(p => p.CountryNavigation)
                            .ToList();
                    }

                    return cartFavoritesService.SetOptions(loadedProducts);
                }
            });


            await Task.Delay(100);

            await Dispatcher.InvokeAsync(() =>
            {
                products = productsFromDb;
                listViewProducts.ItemsSource = products;

                LoadService();
                LoadValues();

                loadingGrid.Visibility = Visibility.Collapsed;
                cardFilters.Visibility = Visibility.Visible;
                listViewProducts.Visibility = Visibility.Visible;
            });
        }
        private void LoadValues()
        {
            if (products == null || !products.Any())
            {
                minPrice = 0;
                maxPrice = 0;
                tbMinPrice.Text = "0";
                tbMaxPrice.Text = "0";
                return;
            }

            maxPrice = products.Max(p => p.CostProduct);
            minPrice = products.Min(p=> p.CostProduct);

            tbMaxPrice.Text = maxPrice.ToString();
            tbMinPrice.Text = minPrice.ToString();

        }
        private void btnProductName_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void tbProductName_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;
            if(hyperlink.DataContext is Product product)
            {
                listViewProducts.SelectedItem = product;
                MainWindow main = Application.Current.MainWindow as MainWindow;
                main.cigarFrame.Navigate(new Pages.ProductPage(listViewProducts.SelectedItem as Product, productStatus, PageType.ListProductPage));
            }
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.DataContext is Product product)
            {
                listViewProducts.SelectedItem = product;
                MainWindow main = Application.Current.MainWindow as MainWindow;
                main.cigarFrame.Navigate(new Pages.ProductPage(listViewProducts.SelectedItem as Product, productStatus, PageType.ListProductPage));
            }
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }
        private async Task LoadFiltersAsync()
        {
            using(CigarhouseContext _context =  new CigarhouseContext())
            {
                lbBrand.DisplayMemberPath = "Name";
                lbCountry.DisplayMemberPath = "CountryName";
                brands = await _context.Brands.ToListAsync();
                countries = await _context.Countries.ToListAsync();

                brands.Insert(0, new Brand { BrandId = 0, Name = "Все" });
                countries.Insert(0, new Country { CountryId = 0, CountryName = "Все" });

                lbBrand.ItemsSource = brands;
                lbCountry.ItemsSource = countries;
            }
        }

        private void lbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lbCountry.SelectedItem is Country selected)
            {
                productFilter.SelectedCountry = selected.CountryId == 0 ? null : selected;
                categoryFreezer.Execute();
            }
        }

        private void lbBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbBrand.SelectedItem is Brand selected)
            {
                productFilter.SelectedBrand = selected.BrandId == 0 ? null : selected;
                categoryFreezer.Execute();
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            productFilter.SearchText = tbSearch.Text;
            searchFreezer.Execute();


        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void sliderPrice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void tbMinPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbMinPrice.Text))
            {
                try
                {
                    productFilter.MinPrice = Convert.ToDecimal(tbMinPrice.Text);
                    priceFreezer.Execute();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Введите числа!");
                }
            }

        }


        private void tbMaxPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbMaxPrice.Text)) { 
                try
                {
                    productFilter.MaxPrice = Convert.ToDecimal(tbMaxPrice.Text);
                    priceFreezer.Execute();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Введите числа!");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


        private void tbBuyButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            Button button = sender as Button;
            if (button.DataContext is Product product)
            {
                main.currentUser.CartNavigation.Products.Add(product);
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

        private void minusButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button.DataContext is Product product)
            {
                if (product.PurchaseAmount >0)
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

