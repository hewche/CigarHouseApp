using CigarHouseApp.Models;
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

namespace CigarHouseApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListProductsPage.xaml
    /// </summary>
    public partial class ListProductsPage : Page
    {

        List<Product> products = new List<Product>();
        List<Brand> brands = new List<Brand>();
        List<Product> filteredProducts = new List<Product>();
        List<Country> countries = new List<Country>();


        private Brand selectedBrand = null;
        private Country selectedCountry = null;

        enum Filters
        {
            All,
            Brand,
            Country,
            None
        }
        public ListProductsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProducts();
            LoadFilters();
        }


        private void LoadProducts()
        {
            using (var _context = new CigarhouseContext())
            {
                products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Cigar)
                .Include(p=> p.CountryNavigation)
                .ToList();

                listViewProducts.ItemsSource = products;
            }
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
                main.cigarFrame.Navigate(new Pages.ProductPage(listViewProducts.SelectedItem as Product));
            }
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.DataContext is Product product)
            {
                listViewProducts.SelectedItem = product;
                MainWindow main = Application.Current.MainWindow as MainWindow;
                main.cigarFrame.Navigate(new Pages.ProductPage(listViewProducts.SelectedItem as Product));
            }
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }
        private void LoadFilters()
        {
            using(CigarhouseContext _context =  new CigarhouseContext())
            {
                lbBrand.DisplayMemberPath = "Name";
                lbCountry.DisplayMemberPath = "CountryName";
                brands = _context.Brands.ToList();
                countries = _context.Countries.ToList();

                brands.Insert(0, new Brand { BrandId = 0, Name = "Все" });
                countries.Insert(0, new Country { CountryId = 0, CountryName = "Все" });

                lbBrand.ItemsSource = brands;
                lbCountry.ItemsSource = countries;
            }
        }
        private void tbFilters_Click(object sender, RoutedEventArgs e)
        {
            if(additionFilters.Visibility == Visibility.Hidden) 
                additionFilters.Visibility = Visibility.Visible;
            else
                additionFilters.Visibility = Visibility.Hidden;
        }

        private void lbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lbCountry.SelectedItem is Country selected)
            {
                selectedCountry = selected.CountryId == 0 ? null : selected;
                ApplyFilters();
            }
        }

        private void lbBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbBrand.SelectedItem is Brand selected)
            {
                selectedBrand = selected.BrandId == 0 ? null : selected;
                ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            // Начинаем с полного списка
            var query = products.AsEnumerable();

            // Применяем фильтр по бренду, если выбран
            if (selectedBrand != null)
            {
                query = query.Where(p => p.BrandId == selectedBrand.BrandId);
            }

            // Применяем фильтр по стране, если выбрана
            if (selectedCountry != null)
            {
                query = query.Where(p => p.Country == selectedCountry.CountryId);
            }

            filteredProducts = query.ToList();
            listViewProducts.ItemsSource = filteredProducts;
        }
    }
}
