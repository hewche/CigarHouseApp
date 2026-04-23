using CigarHouseApp.Models;
using ControlzEx.Standard;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CigarHouseApp.Pages
{
    public partial class OrdersPage : Page
    {
        private List<Order> _orders;
        private User _user;

        public OrdersPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadOrders();
            itemsControlOrders.ItemsSource = _orders;
        }
        private void LoadOrders()
        {
            using (CigarhouseContext context = new CigarhouseContext())
            {
                _orders = context.Orders
                    .Where(o => o.UserId == _user.UserId)
                    .Include(u => u.OrderItems)
                        .ThenInclude(p => p.Product)
                    .Include(o => o.OrderStatus).ToList();
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tbOrdersCount.Text = $"{_orders.Count} заказов";
        }


        private void btnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.Tag as Order;

            if (order == null) return;
            if(order.OrderStatus.OrderStatusId == 5)
            {
                MessageBox.Show("Заказ уже отменен");
                return;
            }
            var result = MessageBox.Show($"Вы уверены, что хотите отменить заказ №{order.OrderId}?",
                "Подтверждение отмены", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new CigarhouseContext())
                    {
                        var orderToUpdate = context.Orders.FirstOrDefault(o => o.OrderId == order.OrderId);
                        if (orderToUpdate != null)
                        {
                            orderToUpdate.OrderStatusId = 5;
                            orderToUpdate.UpdatedAt = DateTime.Now;
                            context.SaveChanges();
                        }
                    }

                    order.OrderStatusId = 5;
                    if (order.OrderStatus != null)
                        order.OrderStatus.Name = "Отменен";

                    itemsControlOrders.ItemsSource = null;
                    itemsControlOrders.ItemsSource = _orders;

                    MessageBox.Show($"Заказ №{order.OrderId} отменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}