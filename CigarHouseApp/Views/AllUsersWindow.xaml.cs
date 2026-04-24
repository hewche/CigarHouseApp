using CigarHouseApp.Helpers;
using CigarHouseApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CigarHouseApp.Views
{
    public partial class AllUsersWindow : Window
    {
        private readonly ImageService _imageService = new ImageService();
        private readonly int _currentUserId;
        private List<User> _users = new();

        public AllUsersWindow(int currentUserId)
        {
            InitializeComponent();
            _ = _imageService;
            _currentUserId = currentUserId;
            LoadUsers();
        }

        private void LoadUsers()
        {
            using var context = new CigarhouseContext();
            _users = context.Users
                .AsNoTracking()
                .Where(u => u.RoleId == 2)
                .OrderBy(u => u.Login)
                .ToList();

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string searchText = tbSearchLogin.Text?.Trim() ?? string.Empty;

            var filteredUsers = _users
                .Where(u => string.IsNullOrEmpty(searchText) ||
                            (u.Login?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false))
                .ToList();

            lvUsers.ItemsSource = filteredUsers;
        }

        private void tbSearchLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not int userId)
            {
                return;
            }

            if (userId == _currentUserId)
            {
                MessageBox.Show("Нельзя удалить текущего авторизованного пользователя.");
                return;
            }

            if (MessageBox.Show("Удалить этого пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            using var context = new CigarhouseContext();
            using var transaction = context.Database.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == userId);

                if (user is null)
                {
                    transaction.Rollback();
                    MessageBox.Show("Пользователь уже удален.");
                    LoadUsers();
                    return;
                }

                int? cartId = context.Usercarts
                    .Where(c => c.UserId == userId)
                    .Select(c => (int?)c.CartId)
                    .FirstOrDefault();

                int? favoritesId = context.Userfavorites
                    .Where(f => f.UserId == userId)
                    .Select(f => (int?)f.FavoritesId)
                    .FirstOrDefault();

                if (cartId.HasValue)
                {
                    context.Database.ExecuteSqlInterpolated(
                        $"delete from itemcart where cart_id = {cartId.Value}");
                }

                if (favoritesId.HasValue)
                {
                    context.Database.ExecuteSqlInterpolated(
                        $"delete from itemfavorites where favorites_id = {favoritesId.Value}");
                }

                context.Database.ExecuteSqlInterpolated(
                    $@"delete from order_item
                       where order_id in (select order_id from orders where user_id = {userId})");

                context.Database.ExecuteSqlInterpolated(
                    $"delete from orders where user_id = {userId}");

                context.Database.ExecuteSqlInterpolated(
                    $"delete from review where user_id = {userId}");

                context.Database.ExecuteSqlInterpolated(
                    $"delete from news where author_id = {userId}");

                user.Cart = null;
                user.Favorites = null;
                context.SaveChanges();

                if (cartId.HasValue)
                {
                    context.Database.ExecuteSqlInterpolated(
                        $"delete from usercart where user_id = {userId}");
                }

                if (favoritesId.HasValue)
                {
                    context.Database.ExecuteSqlInterpolated(
                        $"delete from userfavorites where user_id = {userId}");
                }

                context.Database.ExecuteSqlInterpolated(
                    $"delete from users where user_id = {userId}");

                transaction.Commit();
                LoadUsers();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show($"Не удалось удалить пользователя: {ex.Message}");
            }
        }
    }
}
