using CigarHouseApp;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CigarHouseApp.Views
{
    /// <summary>
    /// Логика взаимодействия для CheckoutWindow.xaml
    /// </summary>
    public partial class CheckoutWindow : Window
    {
        List<Product> orderProducts = new List<Product>();
        MainWindow _main = Application.Current.MainWindow as MainWindow;
        decimal total = 0;
        decimal subTotal = 0;
        decimal deliveryTotal = 0;
        public CheckoutWindow(List<Product> products, decimal total, decimal subTotal, decimal deliveryTotal)
        {
            InitializeComponent();
            orderProducts = products;
            this.total = total;
            this.subTotal = subTotal;
            this.deliveryTotal = deliveryTotal;
        }

        
        private void btnConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFields())
            {
                MessageBox.Show("Заполните необходимые поля!");
                return;
            }
            try
            {
                CreateOrder();
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //this.DialogResult = false;
            }
        }

        private void CreateOrder()
        {
            using (CigarhouseContext context = new CigarhouseContext())
            {
                Order order = new Order();
                foreach (Product product in orderProducts)
                {
                    order.OrderItems.Add(new OrderItem() { ProductId = product.ProductId, OrderId = order.OrderId, Quantity = product.PurchaseAmount });
                }
                order.OrderStatusId = 1;
                order.UserId = _main.currentUser.UserId;
                order.UpdatedAt = DateTime.Now;
                order.Address = $"{tbCity.Text} {tbStreet.Text} {tbHouse.Text} {tbEntrance.Text} {tbApartment.Text}";
                if (rbCard.IsChecked == true)
                    order.PaymentMethod = 1;
                else if (rbCash.IsChecked == true)
                    order.PaymentMethod = 2;
                else if (rbSbp.IsChecked == true)
                    order.PaymentMethod = 3;

                context.Orders.Add(order);
                context.SaveChanges();
            }
        }
        private bool CheckFields()
        {
            return !string.IsNullOrEmpty(tbName.Text)
                && !string.IsNullOrEmpty(tbPhone.Text)
                && !string.IsNullOrEmpty(tbCity.Text)
                && !string.IsNullOrEmpty(tbStreet.Text)
                && !string.IsNullOrEmpty(tbHouse.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbTotal.Text = Math.Round(total).ToString();
            tbSubtotal.Text = Math.Round(subTotal).ToString();
            tbDelivery.Text = Math.Round(deliveryTotal).ToString();
        }
    }
}
