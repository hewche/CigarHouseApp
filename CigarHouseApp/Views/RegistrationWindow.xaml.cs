using CigarHouseApp.Helpers;
using CigarHouseApp.Models;
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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        CigarhouseContext _context;
        
        public RegistrationWindow()
        {
            InitializeComponent();
            _context = new CigarhouseContext();
        }

        private async void btnRegister_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (!CheckFields())
            {
                if (pbPassword.Password == pbConfirmPassword.Password)
                {
                    await AddUser(new User() { FirstName = tbFirstName.Text, Login = tbLogin.Text, Password = PasswordHasher.GetSHA512Hash(pbPassword.Password), RoleId = 2 , Phone="111-222"});
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают!");
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private bool CheckFields()
        {
            return !string.IsNullOrWhiteSpace(tbFirstName.Text) &&
                   !string.IsNullOrWhiteSpace(tbLastName.Text) &&
                   !string.IsNullOrWhiteSpace(tbLogin.Text) &&
                   !string.IsNullOrWhiteSpace(tbEmail.Text) &&
                   !string.IsNullOrWhiteSpace(pbPassword.Password) &&
                   !string.IsNullOrWhiteSpace(pbConfirmPassword.Password);
        }

        private async Task AddUser(User user)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var cart = new Usercart();
                var favorites = new Userfavorite();

                user.Usercart = cart;
                user.Userfavorite = favorites;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                MessageBox.Show("Пользователь добавлен!");
            }
            catch(Exception ex) 
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"{ex.Message}");
                MessageBox.Show(ex.InnerException?.Message);
            }
        }
    }
}
