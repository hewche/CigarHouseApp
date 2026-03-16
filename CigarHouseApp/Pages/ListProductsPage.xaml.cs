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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CigarHouseApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListProductsPage.xaml
    /// </summary>
    public partial class ListProductsPage : Page
    {

        List<Product> products = new List<Product>();
        public ListProductsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (CigarhouseContext _context = new CigarhouseContext())
            {
                products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Cigar)
                .ToList();
                listViewProducts.ItemsSource = products;
            }
        }
    }
}
