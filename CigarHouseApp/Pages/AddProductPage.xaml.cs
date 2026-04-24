using CigarHouseApp.Helpers;
using CigarHouseApp.Models;
using CigarHouseApp.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CigarHouseApp.Pages
{
    public partial class AddProductPage : Page
    {
        ImageService _imageService;
        string? _selectedImage = null;
        private class ProductTypeItem
        {
            public string Name { get; set; } = string.Empty;
            public bool IsCigar { get; set; }
        }
        public AddProductPage()
        {
            InitializeComponent();
            Loaded += AddProductPage_Loaded;
            _imageService = new ImageService();
        }

        private void AddProductPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadValues();
        }

        private void LoadValues()
        {
            using (var context = new CigarhouseContext())
            {
                cbBrand.ItemsSource = context.Brands.OrderBy(b => b.Name).ToList();
                cbCountry.ItemsSource = context.Countries.OrderBy(c => c.CountryName).ToList();
            }

            cbProductType.ItemsSource = new List<ProductTypeItem>
            {
                new ProductTypeItem { Name = "Сигара", IsCigar = true },
                new ProductTypeItem { Name = "Аксессуар", IsCigar = false }
            };
            cbProductType.SelectedIndex = 0;
        }


        private void btnLoadImage_Click(object sender, RoutedEventArgs e)
        {

            string filename = _imageService.UploadPhoto("ImagesDB");
            if (!string.IsNullOrEmpty(filename))
            {
                UpdateImage(filename);
            }
            _selectedImage = filename;
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!TryParseForm(out var productName, out var cost, out var quantity, out var brandId, out var countryId, out var isCigar))
            {
                return;
            }

            try
            {
                SaveProduct(productName, cost, quantity, brandId, countryId, isCigar);

                MessageBox.Show("Товар успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveProduct(string productName, decimal cost, int quantity, int brandId, int countryId, bool isCigar)
        {
            using (var context = new CigarhouseContext())
            {
                var nextProductId = context.Products
                    .Select(p => p.ProductId)
                    .Max() + 1;

                var product = new Product
                {
                    ProductId = nextProductId,
                    ProductName = productName,
                    CostProduct = cost,
                    Quantity = quantity,
                    BrandId = brandId,
                    Country = countryId,
                    Image = _selectedImage
                };

                context.Products.Add(product);
                context.SaveChanges();

                if (isCigar)
                {
                    context.Cigars.Add(new Cigar { ProductId = product.ProductId });
                }
                else
                {
                    context.Accessories.Add(new Accessory { ProductId = product.ProductId });
                }

                context.SaveChanges();
            }
        }

        private bool TryParseForm(out string productName, out decimal cost, out int quantity, out int brandId, out int countryId, out bool isCigar)
        {
            productName = tbProductName.Text?.Trim() ?? string.Empty;
            cost = 0;
            quantity = 0;
            brandId = 0;
            countryId = 0;
            isCigar = true;

            if (string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("Введите название товара.");
                return false;
            }

            if (!decimal.TryParse(tbCost.Text, out cost) || cost <= 0)
            {
                MessageBox.Show("Введите корректную цену.");
                return false;
            }

            if (!int.TryParse(tbQuantity.Text, out quantity) || quantity < 0)
            {
                MessageBox.Show("Введите корректное количество.");
                return false;
            }

            if (cbBrand.SelectedItem is not Brand brand)
            {
                MessageBox.Show("Выберите марку.");
                return false;
            }

            if (cbCountry.SelectedItem is not Country country)
            {
                MessageBox.Show("Выберите страну.");
                return false;
            }

            if (cbProductType.SelectedItem is not ProductTypeItem productType)
            {
                MessageBox.Show("Выберите тип товара.");
                return false;
            }

            brandId = brand.BrandId;
            countryId = country.CountryId;
            isCigar = productType.IsCigar;
            return true;
        }


        private void UpdateImage(string filename)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\ImagesDB\" + filename);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            imgPreview.Source = bitmap;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            tbProductName.Clear();
            tbCost.Clear();
            tbQuantity.Clear();
            tbSelectedImage.Text = string.Empty;
            imgPreview.Source = null;
            _selectedImage = null;

            if (cbBrand.Items.Count > 0)
            {
                cbBrand.SelectedIndex = 0;
            }

            if (cbCountry.Items.Count > 0)
            {
                cbCountry.SelectedIndex = 0;
            }

            cbProductType.SelectedIndex = 0;
        }

        private void addBrandlink_Click(object sender, RoutedEventArgs e)
        {
            AddBrandWindow addBrandWindow = new AddBrandWindow();
            if(addBrandWindow.ShowDialog()==true)
            {
                MessageBox.Show("Бренд добавлен");
                LoadValues();
            }
            else
            {
                return;
            }
        }
    }
}
