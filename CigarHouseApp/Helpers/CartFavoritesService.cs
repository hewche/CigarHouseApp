using CigarHouseApp.Models;
using CigarHouseApp.Views;
using Microsoft.EntityFrameworkCore;
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
        CigarhouseContext _context;
        public CartFavoritesService()
        {
            _mainWindow = Application.Current.MainWindow as MainWindow;
            _context= new CigarhouseContext();
        }

        private User? LoadUser()
        {
            return _context.Users
                .Include(u => u.Userfavorite)
                .ThenInclude(uf => uf.Products)
                .Include(u=>u.Usercart)
                .ThenInclude(uc=>uc.Products)
                .FirstOrDefault(u => u.UserId == _mainWindow.currentUser.UserId);
        }

        public void ToggleFavorites(Product product)
        {

            var userDb = LoadUser();
            if(userDb==null)
            {
                userDb = _mainWindow.currentUser;
            }

            var existingProduct = userDb.Userfavorite.Products
                .FirstOrDefault(p => p.ProductId == product.ProductId);

            if (existingProduct != null)
            {
                userDb.Userfavorite.Products.Remove(existingProduct);
                existingProduct.IsFavorite = false;
            }
            else
            {
                var productDb = _context.Products.Find(product.ProductId);
                productDb.IsFavorite = true;
                userDb.Userfavorite.Products.Add(productDb);
            }

            product.IsFavorite = existingProduct == null;
            _context.SaveChanges();
            _mainWindow.currentUser = userDb;

        }

        public void TogglePurchase(Product product)
        {
            var userDb = LoadUser();
            if (userDb == null)
            {
                userDb = _mainWindow.currentUser;
            }

            var existingProduct = userDb.Usercart.Products
                .FirstOrDefault(p => p.ProductId == product.ProductId);

            if (existingProduct != null)
            {
                userDb.Userfavorite.Products.Remove(existingProduct);
                existingProduct.IsPurchase = false;
            }
            else
            {
                var productDb = _context.Products.Find(product.ProductId);
                productDb.IsPurchase = true;
                userDb.Userfavorite.Products.Add(productDb);
            }

            product.IsPurchase = existingProduct == null;
            _context.SaveChanges();
            _mainWindow.currentUser = userDb;

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
