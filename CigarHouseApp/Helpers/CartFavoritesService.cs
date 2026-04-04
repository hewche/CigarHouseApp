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
                existingProduct.IsFavorite = false;
            }
            else
            {
                _mainWindow.currentUser.FavoritesNavigation.Products.Add(product);
                product.IsFavorite = true;
            }
        }

        public List<Product> SetFavorites(List<Product> products)
        {
            List<Product> favoritesProducts = _mainWindow.currentUser.FavoritesNavigation.Products.ToList();
            if (favoritesProducts == null || !favoritesProducts.Any())
                return products;

            var favoriteIds = favoritesProducts.Select(f => f.ProductId).ToHashSet();

            foreach (var product in products)
            {
                product.IsFavorite = favoriteIds.Contains(product.ProductId);
            }

            return products;
        }
    }
}
