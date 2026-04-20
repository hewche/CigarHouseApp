using CigarHouseApp.Helpers;
using CigarHouseApp.Models;
using CigarHouseApp.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CigarHouseApp.Helpers.ProductFilter;
using static CigarHouseApp.Views.MainWindow;

namespace CigarHouseApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        MainWindow _main;
        List<Review> reviews = new List<Review>();
        ProductStatus _status;
        PageType _previousPage;
        Product _currentProduct;
        CartFavoritesService cartFavoritesService = new CartFavoritesService();
        public ProductPage()
        {
            InitializeComponent();
        }

        public ProductPage(Product product, ProductStatus status, PageType previousPage)
        {
            InitializeComponent();
            RatingComboBox.ItemsSource = new List<int> { 1, 2, 3, 4, 5 };
            _currentProduct = product;
            _status = status;
            _previousPage = previousPage;
            LoadDataProduct(_currentProduct);
            _main = Application.Current.MainWindow as MainWindow;
        }

        private void likeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentProduct != null)
            {
                cartFavoritesService.ToggleFavorites(_currentProduct);
            }
        }

        private void LoadDataProduct(Product product)
        {
            if(_status == ProductStatus.ACCESSORY)
            {
                svStatsCigar.ContentTemplate = this.FindResource("AccessoryStatsTemplate") as DataTemplate;
                svStatsCigar.Content = product;
            }
            else
            {
                svStatsCigar.ContentTemplate = this.FindResource("CigarStatsTemplate") as DataTemplate;
                svStatsCigar.Content = product;
            }
            spProductInfo.DataContext = product;
            tbBrandName.DataContext = product;
            addToCart.DataContext = product;
            LoadReviews(product);
        }

        private void LoadReviews(Product product)
        {
            using(CigarhouseContext _context = new CigarhouseContext())
            {
                reviews = _context.Reviews.Include(r=> r.User)
                    .Where(r => r.ProductId == product.ProductId).ToList();
                listReview.ItemsSource = reviews;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                BackToPrevious();
            }
        }

        private void BackToPrevious()
        {
            //MainWindow main = Application.Current.MainWindow as MainWindow;
            switch (_previousPage)
            {
                case PageType.FavoritesPage:
                    _main.cigarFrame.Navigate(new FavoritesPage(_main.currentUser.Userfavorite.Products.ToList()));
                    break;
                case PageType.ListProductPage:
                    _main.cigarFrame.Navigate(new ListProductsPage(_status));
                    break;
            }
        }

        private void UpdateItemContext(FrameworkElement item)
        {
            var temp = item.DataContext;
            item.DataContext = null;
            item.DataContext = temp;
        }
        private void addToCart_Click(object sender, RoutedEventArgs e)
        {
            if (_currentProduct != null)
            {
                cartFavoritesService.TogglePurchase(_currentProduct);
                if(sender is Button button)
                {
                    UpdateItemContext(button);
                }
            }
        }

        private void btnAddReview_Click(object sender, RoutedEventArgs e)
        {
            

            if (_main?.currentUser == null || _main.currentUser.Login == "unknown")
            {
                MessageBox.Show("Чтобы оставить отзыв, нужно авторизоваться.");
                return;
            }

            if (_currentProduct == null)
            {
                MessageBox.Show("Не удалось определить товар для отзыва.");
                return;
            }

            string reviewText = tbWriteReview.Text ?? string.Empty;
            string headerText = tbHeader.Text;
            if (string.IsNullOrWhiteSpace(reviewText))
            {
                MessageBox.Show("Введите текст отзыва.");
                return;
            }

            if (reviewText.Length > 300)
            {
                MessageBox.Show("Отзыв не должен превышать 300 символов.");
                return;
            }
            if (headerText.Length > 100)
            {
                MessageBox.Show("Заголовок не должен превышать 300 символов.");
                return;
            }
            if(RatingComboBox.SelectedItem == null)
            {
                MessageBox.Show("Чтобы оставить отзыв необходимо оценить товар");
                return;
            }

            try
            {
                SaveReview();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении отзыва: {ex.Message}");
            }
        }

        private void SaveReview()
        {
            using (CigarhouseContext context = new CigarhouseContext())
            {
                Review review = new Review
                {
                    UserId = _main.currentUser.UserId,
                    ProductId = _currentProduct.ProductId,
                    Title = string.IsNullOrWhiteSpace(tbHeader.Text) ? "Отзыв на товар" : tbHeader.Text,
                    ReviewText = tbWriteReview.Text,
                    Rating = (int)RatingComboBox.SelectedItem,
                    CreatedAt = DateTime.Now
                };

                context.Reviews.Add(review);
                context.SaveChanges();
            }

            tbWriteReview.Clear();
            tbHeader.Clear();
            RatingComboBox.SelectedIndex = -1;
            LoadReviews(_currentProduct);
        }

    }
}
