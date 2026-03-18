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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Frame frame;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tc && tc.IsLoaded)
            {
                switch (tabControlProducts.SelectedIndex)
                {
                    case 0:
                        cigarFrame.Navigate(new Pages.ListProductsPage());
                        break;
                }
            }
        }

        private void tabControlProducts_Selected(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void tabControlProducts_Loaded(object sender, RoutedEventArgs e)
        {
            tabControlProducts.SelectedIndex = -1;
        }
    }
}
