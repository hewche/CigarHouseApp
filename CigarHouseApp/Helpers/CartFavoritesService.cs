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
            var existingProduct = _mainWindow.currentUser.Userfavorite.Products
                .FirstOrDefault(p => p.ProductId == product.ProductId);

            if (existingProduct != null)
            {
                _mainWindow.currentUser.Userfavorite.Products.Remove(existingProduct);
                existingProduct.IsFavorite = false;
                product.IsFavorite = false;
            }
            else
            {
                _mainWindow.currentUser.Userfavorite.Products.Add(product);
                product.IsFavorite = true;
            }
        }

        public void TogglePurchase(Product product)
        {
            var existingProduct = _mainWindow.currentUser.Usercart.Products
                .FirstOrDefault(p => p.ProductId == product.ProductId);

            if (existingProduct != null)
            {
                _mainWindow.currentUser.Usercart.Products.Remove(existingProduct);
                existingProduct.IsPurchase = false;
                product.IsPurchase = false;
            }
            else
            {
                if(product.PurchaseAmount == 0)
                {
                    product.PurchaseAmount = 1;
                }
                _mainWindow.currentUser.Usercart.Products.Add(product);
                product.IsPurchase = true;
            }
        }

        private List<Product> SetFavorites(List<Product> products)
        {
            List<Product> favoritesProducts = _mainWindow.currentUser.Userfavorite.Products.ToList();
            if (favoritesProducts == null || !favoritesProducts.Any())
                return products;

            var favoriteIds = favoritesProducts.Select(f => f.ProductId).ToHashSet();

            foreach (var product in products)
            {
                product.IsFavorite = favoriteIds.Contains(product.ProductId);
            }

            return products;
        }


        private List<Product> SetPurchase(List<Product> products)
        {
            List<Product> cartProducts = _mainWindow.currentUser.Usercart.Products.ToList();
            if (cartProducts == null || !cartProducts.Any())
                return products;

            var cartIds = cartProducts.Select(f => f.ProductId).ToHashSet();

            foreach (var product in products)
            {
                product.IsPurchase = cartIds.Contains(product.ProductId);
            }

            return products;
        }

        public List<Product> SetOptions(List<Product> products)
        {
            return SetPurchase(SetFavorites(products));
        }
    }
}
