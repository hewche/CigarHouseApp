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
    /// Логика взаимодействия для UserUpdateProfileWindow.xaml
    /// </summary>
    public partial class UserUpdateProfileWindow : Window
    {
        User _currentUser;
        bool IsPasswordChanged = false;
        public UserUpdateProfileWindow(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            LoadData();
        }
        private void LoadData()
        {
            svUser.DataContext = _currentUser;
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPasswordPanel();
        }

        public bool CheckPassword()
        {
            if(PasswordPanel.Visibility == Visibility.Visible)
            {
                if(string.IsNullOrEmpty(pbCurrentPassword.Password) && string.IsNullOrEmpty(pbConfirmPassword.Password) && string.IsNullOrEmpty(pbNewPassword.Password))
                    return false;
                else if(PasswordHasher.VerifySHA512Hash(pbCurrentPassword.Password,_currentUser.Password) && pbNewPassword.Password == pbConfirmPassword.Password)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Неверный пароль!");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool CheckFields()
        {
            return !string.IsNullOrEmpty(tbFirstName.Text)
                && !string.IsNullOrEmpty(tbLastName.Text)
                && !string.IsNullOrEmpty(tbLogin.Text)
                && !string.IsNullOrEmpty(dpBirthDate.SelectedDate.ToString())
                && !string.IsNullOrEmpty(tbPhone.Text);
        }
        private void ShowPasswordPanel()
        {
            if(PasswordPanel.Visibility == Visibility.Visible)
                PasswordPanel.Visibility = Visibility.Collapsed;
            else
                PasswordPanel.Visibility = Visibility.Visible;
        }

        private void UpdateUser()
        {
            using (var context = new CigarhouseContext())
            {
                _currentUser.FirstName = tbFirstName.Text;
                _currentUser.LastName = tbLastName.Text;
                _currentUser.Phone = tbPhone.Text;
                _currentUser.Email = tbEmail.Text;

                context.Users.Update(_currentUser);
                context.SaveChanges();
            }
        }

        private void UpdatePassword()
        {
            _currentUser.Password = PasswordHasher.GetSHA512Hash(pbNewPassword.Password);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckPassword())
            {
                UpdatePassword();
            }
            if (CheckFields())
            {
                try
                {
                    UpdateUser();
                    this.DialogResult = true;
                }
                catch (Exception ex)
                {
                    this.DialogResult = false;
                    MessageBox.Show("Пользователь не был обновлен");
                }
            }
        }

    }
}
