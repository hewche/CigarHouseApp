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
    /// Логика взаимодействия для AuthentificationWindow.xaml
    /// </summary>
    public partial class AuthentificationWindow : Window
    {
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
            MainWindow main = new MainWindow();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = main;
            if (main.ShowDialog() == true)
            {

            }
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
