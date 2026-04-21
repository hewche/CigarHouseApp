using CigarHouseApp.Pages;
using CigarHouseApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CigarHouseApp.Helpers
{
    public class NavigateService
    {
        MainWindow main = Application.Current.MainWindow as MainWindow;
        public enum ProductStatus
        {
            CIGAR,
            ACCESSORY
        }

        public enum PageType
        {
            FavoritesPage,
            ListProductPage,
            ListProductPageAccessories,
            BestProductsPage
        }

        public void BackToPrevious(PageType page)
        {
            switch (page)
            {
                case PageType.FavoritesPage:
                    main.cigarFrame.Navigate(new FavoritesPage(main.currentUser.Userfavorite.Products.ToList()));
                    break;
                case PageType.ListProductPage:
                    main.cigarFrame.Navigate(new ListProductsPage(ProductStatus.CIGAR));
                    break;
                case PageType.BestProductsPage:
                    main.cigarFrame.Navigate(new BestProductsPage());
                    break;
                case PageType.ListProductPageAccessories:
                    main.cigarFrame.Navigate(new ListProductsPage(ProductStatus.ACCESSORY));
                    break;
            }
        }
    }
}
