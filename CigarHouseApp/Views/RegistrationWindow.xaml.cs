using CigarHouseApp.Helpers;
using CigarHouseApp.Models;
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
          
            if (CheckFields())
            {
                if (pbPassword.Password == pbConfirmPassword.Password)
                {
                    if (CalculateAge(dpBirthDate.SelectedDate.Value)>=18)
                    {
                        await AddUser(new User() { FirstName = tbFirstName.Text, LastName = tbLastName.Text, Login = tbLogin.Text, Email = tbEmail.Text, Password = PasswordHasher.GetSHA512Hash(pbPassword.Password), RoleId = 2, Phone = tbPhone.Text, Birthday = dpBirthDate.SelectedDate });
                    }
                    else
                    {
                        MessageBox.Show("Неправильный возраст!");
                    }
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
                   !string.IsNullOrEmpty(pbConfirmPassword.Password) &&
                   !string.IsNullOrEmpty(dpBirthDate.SelectedDate.ToString());
        }

        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
                age--;

            return age;
        }
        private async Task AddUser(User user)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (_context.Users.FirstOrDefault(u => u.Login ==tbLogin.Text) != null)
                {
                    MessageBox.Show("Логин занят");
                    return;
                }
                var cart = new Usercart();
                var favorites = new Userfavorite();

                user.Usercart = cart;
                user.Userfavorite = favorites;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();


                await transaction.CommitAsync();
                MessageBox.Show("Пользователь добавлен!");
                this.DialogResult = true;

            }
            catch (Exception ex) 
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"{ex.Message}");
                MessageBox.Show(ex.InnerException?.Message);
            }
        }
    }
}
