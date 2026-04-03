using CigarHouseApp.Models;
using CigarHouseApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CigarHouseApp.Helpers
{
    public class CartFavoritesService
    {
        MainWindow _mainWindow;
        public CartFavoritesService()
        {
            _mainWindow = Application.Current.MainWindow as MainWindow;
        }

        public void ToggleFavorites(Product product)
        {
            var existingProduct = _mainWindow.currentUser.FavoritesNavigation.Products
                .FirstOrDefault(p => p.ProductId == product.ProductId);

            if (existingProduct != null)
            {
                _mainWindow.currentUser.FavoritesNavigation.Products.Remove(existingProduct);
            }
            else
            {
                _mainWindow.currentUser.FavoritesNavigation.Products.Add(product);
            }
        }
    }
}
