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
        ImageService _imageService;
        bool IsPasswordChanged = false;
        public UserUpdateProfileWindow(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _imageService = new ImageService();
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
                && !string.IsNullOrEmpty(tbPhone.Text)
                && !string.IsNullOrEmpty(dpBirthDate.SelectedDate.ToString());
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
                if(context.Users.FirstOrDefault(u=>u.Login == _currentUser.Login)!=null)
                {
                    MessageBox.Show("Логин занят");
                    return;
                }
                var user = context.Users.FirstOrDefault(u=>u.UserId == _currentUser.UserId);
                user.FirstName = tbFirstName.Text;
                user.LastName = tbLastName.Text;
                user.Phone = tbPhone.Text;
                user.Login = _currentUser.Login;
                user.Email = tbEmail.Text;
                user.Password = _currentUser.Password;
                user.Birthday = dpBirthDate.SelectedDate;

                context.Users.Update(user);
                context.SaveChanges();
                this.DialogResult = true;

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

        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
                age--;

            return age;
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
                    if (CalculateAge(dpBirthDate.SelectedDate.Value)>=18)
                    {
                      
                        UpdateUser();
                    }
                    else
                    {
                        MessageBox.Show("Неверный возраст");
                    }
                }
                catch (Exception ex)
                {
                    this.DialogResult = false;
                    MessageBox.Show("Пользователь не был обновлен");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }

        private void UploadAvatarButton_Click(object sender, RoutedEventArgs e)
        {
            string filename = _imageService.UploadPhoto("ImagesUser");
            if (!string.IsNullOrEmpty(filename))
            {
                try
                {
                    UpdateAvatar(filename);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.InnerException?.Message);
                }
            }
        }

        private void UpdateAvatar(string filename)
        {
            using (var context = new CigarhouseContext())
            {
                _currentUser.Image = filename;
                var dbUser = context.Users.FirstOrDefault(u => u.UserId == _currentUser.UserId);
                if (dbUser is null)
                {
                    throw new Exception("Пользователь не найден при обновлении аватарки.");
                }

                dbUser.Image = filename;
                context.SaveChanges();
            }
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            string avatarPath = _imageService.GetImagePath("ImagesUser", filename);
            bitmap.UriSource = new Uri(avatarPath);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            imgBrushAvatar.ImageSource = bitmap;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            AuthentificationWindow auth = new AuthentificationWindow();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = auth;
            auth.Show();
            this.Close();
        }
    }
}
