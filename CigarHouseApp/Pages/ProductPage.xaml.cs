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
        List<Review> reviews = new List<Review>();
        ProductStatus _status;
        PageType _previousPage;
        public ProductPage()
        {
            InitializeComponent();
        }

        public ProductPage(Product product, ProductStatus status, PageType previousPage)
        {
            InitializeComponent();
            LoadDataProduct(product);
            _status = status;
            _previousPage = previousPage;
        }

        private void likeButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LoadDataProduct(Product product)
        {
            svStatsCigar.DataContext = product;
            spProductInfo.DataContext = product;
            tbBrandName.DataContext = product;

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
            MainWindow main = Application.Current.MainWindow as MainWindow;
            switch (_previousPage)
            {
                case PageType.FavoritesPage:
                    main.cigarFrame.Navigate(new FavoritesPage(main.currentUser.FavoritesNavigation.Products.ToList()));
                    break;
                case PageType.ListProductPage:
                    main.cigarFrame.Navigate(new ListProductsPage(_status));
                    break;
            }
        }

    }
}
