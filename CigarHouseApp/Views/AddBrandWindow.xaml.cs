using CigarHouseApp.Models;
using CigarHouseApp.Pages;
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
using System.Windows.Shapes;

namespace CigarHouseApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AddBrandWindow.xaml
    /// </summary>
    public partial class AddBrandWindow : Window
    {
        public AddBrandWindow()
        {
            InitializeComponent();
        }

        private void btnAddBrand_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFields())
                return;
            try
            {
                SaveBrand(tbBrandName.Text, tbPhone.Text);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+ex.InnerException?.Message);
            }
        }

        private bool CheckFields()
        {

            if (string.IsNullOrWhiteSpace(tbBrandName.Text))
            {
                MessageBox.Show("Введите название бренда.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbPhone.Text))
            {
                MessageBox.Show("Введите номер телефона.");
                return false;
            }

            return true;
        }
        private void btnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SaveBrand(string name, string phone)
        {
            using(var _context = new CigarhouseContext())
            {
                _context.Brands.Add(new Brand() {Name=name, Phone=phone});
                _context.SaveChanges();
            }
        }
    }
}
