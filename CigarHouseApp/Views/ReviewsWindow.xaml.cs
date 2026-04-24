using CigarHouseApp.Helpers;
using CigarHouseApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CigarHouseApp.Views
{
    public partial class ReviewsWindow : Window
    {
        private readonly ImageService _imageService = new ImageService();

        public ReviewsWindow()
        {
            InitializeComponent();
            _ = _imageService;
            LoadReviews();
        }

        private void LoadReviews()
        {
            using var context = new CigarhouseContext();
            lvReviews.ItemsSource = context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        private void DeleteReview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not int reviewId)
            {
                return;
            }

            if (MessageBox.Show("Удалить этот отзыв?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            using var context = new CigarhouseContext();
            var review = context.Reviews.FirstOrDefault(r => r.ReviewId == reviewId);
            if (review is null)
            {
                MessageBox.Show("Отзыв уже удален.");
                LoadReviews();
                return;
            }

            context.Reviews.Remove(review);
            context.SaveChanges();
            LoadReviews();
        }
    }
}
