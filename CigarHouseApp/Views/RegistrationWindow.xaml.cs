using CigarHouseApp.Helpers;
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
using CigarHouseApp.Models;

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
            if (CheckFields())
            {
                if (pbPassword.Password == pbConfirmPassword.Password)
                {
                    await AddUser(new User() { FirstName = tbFirstName.Text, LastName=tbLastName.Text, Login = tbLogin.Text, Email=tbEmail.Text, Password = PasswordHasher.GetSHA512Hash(pbPassword.Password), RoleId = 2 , Phone=tbPhone.Text, Birthday = dpBirthDate.SelectedDate});
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают!");
                }
            }
            else
            {
                MessageBox.Show("Для регистрации необходимо заполнить все поля!");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private bool CheckFields()
        {
            return !string.IsNullOrEmpty(tbFirstName.Text) &&
                   !string.IsNullOrEmpty(tbLastName.Text) &&
                   !string.IsNullOrEmpty(tbLogin.Text) &&
                   !string.IsNullOrEmpty(tbEmail.Text) &&
                   !string.IsNullOrEmpty(pbPassword.Password) &&
                   !string.IsNullOrEmpty(pbConfirmPassword.Password);
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
