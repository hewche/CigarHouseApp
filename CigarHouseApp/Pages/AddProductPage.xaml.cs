using CigarHouseApp.Helpers;
using CigarHouseApp.Models;
using CigarHouseApp.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CigarHouseApp.Pages
{
    public partial class AddProductPage : Page, INotifyPropertyChanged
    {
        ImageService _imageService;
        Product currentProduct;
        string? _selectedImage = null;
        bool IsUpdate = false;


        private Cigar _cigarStats = new Cigar();
        private Accessory _accessoryStats = new Accessory();

        public Cigar CigarStats
        {
            get => _cigarStats;
            set
            {
                _cigarStats = value;
                OnPropertyChanged();
            }
        }

        public Accessory AccessoryStats
        {
            get => _accessoryStats;
            set
            {
                _accessoryStats = value;
                OnPropertyChanged();
            }
        }
        private class ProductTypeItem
        {
            public string Name { get; set; } = string.Empty;
            public bool IsCigar { get; set; }
        }
        public AddProductPage()
        {
            InitializeComponent();
            LoadValues();

            _imageService = new ImageService();
            currentProduct = new Product();
            tbHeader.Text = "ДОБАВЛЕНИЕ ТОВАРА";
            tbDesc.Text = "Создайте новый товар, загрузите изображение и сохраните в базу.";
        }

        public AddProductPage(Product product)
        {
            InitializeComponent();
            //Loaded -= AddProductPage_Loaded;
            LoadValues();

            _imageService = new ImageService();

            tbHeader.Text = "ИЗМЕНИТЬ ТОВАР";
            tbDesc.Text = "Изменить товар и сохранить в БД";
            btnSaveProduct.Content = "Изменить товар";

            IsUpdate = true;
            currentProduct = product;
            _selectedImage = product.Image;
            if(product != null)
                SetProductValues(product);
        }

        private void SetProductValues(Product product)
        {
            tbProductName.Text = product.ProductName;
            lbBrand.SelectedItem = lbBrand.ItemsSource
                                        .Cast<Brand>()
                                        .FirstOrDefault(b => b.BrandId == product.BrandId);
            lbCountry.SelectedItem = lbCountry.ItemsSource.Cast<Country>().FirstOrDefault(c=>c.CountryId == product.CountryNavigation.CountryId);

            tbCost.Text = product.CostProduct.ToString();
            tbQuantity.Text = product.Quantity.ToString();

            if (product.Cigar != null)
            {
                lbProductType.SelectedIndex = 0;
                _cigarStats = product.Cigar;
                SetProductStats(0);
            }
            else
            {
                lbProductType.SelectedIndex = 1;
                _accessoryStats = product.Accessory;
                SetProductStats(1);
            }


            lbProductType.IsEditable = false;
            tbSelectedImage.Text = product.Image;

            imgPreview.DataContext = product;
        }

        private void SetProductStats(int index)
        {
            if(index == 0)
            {
                spStats.Content = CigarStats;
            }
            else
            {
                spStats.Content = AccessoryStats;
            }
        }

        private void AddProductPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadValues();
        }

        private void LoadValues()
        {
            using (var context = new CigarhouseContext())
            {
                lbBrand.ItemsSource = context.Brands.OrderBy(b => b.Name).ToList();
                lbCountry.ItemsSource = context.Countries.OrderBy(c => c.CountryName).ToList();
            }

            lbProductType.ItemsSource = new List<ProductTypeItem>
            {
                new ProductTypeItem { Name = "Сигара", IsCigar = true },
                new ProductTypeItem { Name = "Аксессуар", IsCigar = false }
            };
            lbProductType.SelectedIndex = 0;
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
                if (IsUpdate)
                {
                    UpdateProduct(productName, cost, quantity, brandId, countryId, isCigar);
                    MessageBox.Show("Товар успешно обновлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    SaveProduct(productName, cost, quantity, brandId, countryId, isCigar);
                    MessageBox.Show("Товар успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении товара: {ex.Message} {ex.InnerException.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateProduct(string productName, decimal cost, int quantity, int brandId, int countryId, bool isCigar)
        {
            using (var context = new CigarhouseContext())
            {
                var product = context.Products.FirstOrDefault(p=>p.ProductId == currentProduct.ProductId);

                product.ProductName = productName;
                product.CostProduct = cost;
                product.Quantity = quantity;
                product.BrandId = brandId;
                product.Country = countryId;
                product.Image = _selectedImage;

                context.Products.Update(product);
                context.SaveChanges();

                if (isCigar)
                {
                    var cigar = context.Cigars.FirstOrDefault(c=>c.ProductId == product.ProductId);
                    if (cigar != null)
                    {
                        cigar.RingGauge = CigarStats.RingGauge;
                        cigar.Strength = CigarStats.Strength;
                        cigar.FlavorProfile = CigarStats.FlavorProfile;
                        cigar.Vitola = CigarStats.Vitola;
                        context.Cigars.Update(cigar);

                    }
                }
                else
                {
                    var accessory = context.Accessories.FirstOrDefault(c => c.ProductId == product.ProductId);
                    if(accessory != null)
                    {
                        accessory.Material=AccessoryStats.Material;
                        accessory.Color = AccessoryStats.Color;
                        context.Accessories.Update(AccessoryStats);

                    }
                }

                context.SaveChanges();
            }
        }

        private void SaveProduct(string productName, decimal cost, int quantity, int brandId, int countryId, bool isCigar)
        {
            using (var context = new CigarhouseContext())
            {
                var transaction = context.Database.BeginTransaction();
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
                {   CigarStats.ProductId = product.ProductId;
                    context.Cigars.Add(CigarStats);
                }
                else
                {
                    AccessoryStats.ProductId = product.ProductId;
                    context.Accessories.Add(AccessoryStats);
                }
                transaction.Commit();
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

            if (lbBrand.SelectedItem is not Brand brand)
            {
                MessageBox.Show("Выберите марку.");
                return false;
            }

            if (lbCountry.SelectedItem is not Country country)
            {
                MessageBox.Show("Выберите страну.");
                return false;
            }

            if (lbProductType.SelectedItem is not ProductTypeItem productType)
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

            if (lbBrand.Items.Count > 0)
            {
                lbBrand.SelectedIndex = -1;
            }

            if (lbCountry.Items.Count > 0)
            {
                lbCountry.SelectedIndex = -1;
            }

            lbProductType.SelectedIndex = -1;
            _accessoryStats = new Accessory();
            _cigarStats = new Cigar();
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

        private void addStatslink_Click(object sender, RoutedEventArgs e)
        {
            if(spStats.Visibility == Visibility.Collapsed)
            {
                spStats.Visibility = Visibility.Visible;
                
            }            
            else
                spStats.Visibility = Visibility.Collapsed;

        }

        private void lbProductType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lbProductType.SelectedIndex == 0)
                spStats.ContentTemplate = this.FindResource("CigarStatsTemplate") as DataTemplate;
            if (lbProductType.SelectedIndex == 1)
                spStats.ContentTemplate = this.FindResource("AccessoryStatsTemplate") as DataTemplate;
            SetProductStats(lbProductType.SelectedIndex);

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
