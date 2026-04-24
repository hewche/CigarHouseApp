using CigarHouseApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CigarHouseApp.Pages
{
    public partial class OrderManagmentPage : Page
    {
        private List<Order> _orders = new List<Order>();
        private List<OrderStatus> _statuses = new List<OrderStatus>();

        public OrderManagmentPage()
        {
            InitializeComponent();
            Loaded += OrderManagmentPage_Loaded;
        }

        private void OrderManagmentPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            lvOrders.ItemsSource = _orders;
        }

        private void LoadData()
        {
            using (var context = new CigarhouseContext())
            {
                _statuses = context.OrderStatuses.OrderBy(s => s.OrderStatusId).ToList();
                _orders = context.Orders
                    .Include(o => o.OrderStatus)
                    .OrderByDescending(o => o.UpdatedAt)
                    .ToList();
            }
            
        }

        private void cbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox comboBox || comboBox.Tag is not Order order)
            {
                return;
            }

            if (comboBox.ItemsSource == null)
            {
                comboBox.ItemsSource = _statuses;
                comboBox.SelectedValue = order.OrderStatusId;
                return;
            }

            if (comboBox.SelectedItem is not OrderStatus selectedStatus)
            {
                return;
            }

            if (order.OrderStatusId == selectedStatus.OrderStatusId)
            {
                return;
            }

            try
            {
                using (var context = new CigarhouseContext())
                {
                    var orderDb = context.Orders.FirstOrDefault(o => o.OrderId == order.OrderId);
                    if (orderDb == null)
                    {
                        return;
                    }

                    orderDb.OrderStatusId = selectedStatus.OrderStatusId;
                    orderDb.UpdatedAt = DateTime.Now;
                    context.SaveChanges();
                }

                order.OrderStatusId = selectedStatus.OrderStatusId;
                order.OrderStatus = selectedStatus;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось изменить статус: {ex.Message}");
            }
        }

        private void cbStatus_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is not ComboBox comboBox || comboBox.Tag is not Order order)
            {
                return;
            }

            comboBox.ItemsSource = _statuses;
            comboBox.SelectedValuePath = nameof(OrderStatus.OrderStatusId);
            comboBox.SelectedValue = order.OrderStatusId;
        }

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not Order order)
            {
                return;
            }

            var result = MessageBox.Show(
                $"Удалить заказ №{order.OrderId}?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                using (var context = new CigarhouseContext())
                {
                    var orderDb = context.Orders
                        .Include(o => o.OrderItems)
                        .FirstOrDefault(o => o.OrderId == order.OrderId);

                    if (orderDb == null)
                    {
                        return;
                    }

                    context.OrderItems.RemoveRange(orderDb.OrderItems);
                    context.Orders.Remove(orderDb);
                    context.SaveChanges();
                }

                _orders.Remove(order);
                lvOrders.ItemsSource = null;
                lvOrders.ItemsSource = _orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось удалить заказ: {ex.Message}");
            }
        }

    }
}
