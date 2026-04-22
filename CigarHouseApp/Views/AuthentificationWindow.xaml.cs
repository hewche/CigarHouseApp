using CigarHouseApp.Helpers;
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
using CigarHouseApp.Models;

namespace CigarHouseApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthentificationWindow.xaml
    /// </summary>
    public partial class AuthentificationWindow : Window
    {
        CigarhouseContext _context = new CigarhouseContext();

        public AuthentificationWindow()
        {
            InitializeComponent();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            if(registrationWindow.ShowDialog() == true )
            {

            }
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbLogin.Text) && !string.IsNullOrEmpty(pbPassword.Password))
            {
                Authorize(tbLogin.Text, pbPassword.Password);
            }
            else
                MessageBox.Show("Введите поля!");
        }

        private void Authorize(string login, string password)
        {
            User user = FindUser(login);
            if (user !=null)
            {
                if (PasswordHasher.VerifySHA512Hash(password, user.Password))
                {
                    MainWindow main = new MainWindow(user);
                    Application.Current.MainWindow.Close();
                    Application.Current.MainWindow = main;
                    if (main.ShowDialog() == true)
                    {
                    }
                }
                else
                {
                    MessageBox.Show("Неверный пароль!");
                }
            }
            else
            {
                MessageBox.Show("Пользователя не существует!");
            }
        }

        private User FindUser(string login)
        {
            return _context.Users
                .Include(u=>u.Usercart)
                .ThenInclude(uc=>uc.Products)
                .Include(u=>u.Userfavorite)
                .ThenInclude(uf => uf.Products)
                .FirstOrDefault(u => u.Login == login);
        }

        private void btnIncognito_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = main;
            if (main.ShowDialog() == true)
            {
                    
            }
        }
    }
}
