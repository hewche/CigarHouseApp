using CigarHouseApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CigarHouseApp.Helpers
{
    public class ProductFilter
    {

        public enum ProductStatus
        {
            CIGAR,
            ACCESSORY
        }

        public enum PageType
        {
            FavoritesPage,
            ListProductPage

        }

        public Brand? SelectedBrand {  get; set; }
        Brand? _tempBrand = null;
        public Country? SelectedCountry { get; set; }
        Country? _tempCountry = null;
        bool isCategoryChanged = false;

        public decimal MaxPrice { get; set; }
        decimal _tempMaxPrice = 0;
        public decimal MinPrice { get; set; }
        decimal _tempMinPrice = 0;

        bool isPriceChanged = false;

        public string SearchText { get; set; } = "";
        private string _tempSearch = "";

        private List<Product> _filteredProducts;

        public async Task<List<Product>> ApplyFilters(List<Product> products)
        {
            await Task.Run(() => {
                var query = products.AsEnumerable();
                if (SelectedBrand != null)
                {
                    if(SelectedBrand != _tempBrand)
                    {
                        isCategoryChanged = true;
                        _tempBrand = SelectedBrand;
                    }
                    query = query.Where(p => p.BrandId == SelectedBrand.BrandId);
                }
                if (SelectedCountry != null)
                {
                    if (SelectedCountry != _tempCountry)
                    {
                        isCategoryChanged = true;
                        _tempCountry = SelectedCountry;
                    }
                    query = query.Where(p => p.Country == SelectedCountry.CountryId);
                }

                if (MinPrice >= 0 || (MaxPrice >= 0 && MinPrice < MaxPrice))
                {
                    if (MinPrice != _tempMinPrice || MaxPrice != _tempMaxPrice)
                    {
                        isPriceChanged = true;
                    }
                    query = PriceFilter(query.ToList());
                }

                _filteredProducts = query.ToList();
            });
            if(_tempSearch == SearchText && !isCategoryChanged)
            {
                isPriceChanged = false;
                isCategoryChanged = false;
                return _filteredProducts;
            }
            else
            {
                _tempSearch = SearchText;
                return SearchProducts(_filteredProducts); ;
            }
        }


        private List<Product> PriceFilter(List<Product> products)
        {
            if (products == null || !products.Any())
                return new List<Product>();

            bool hasMin = MinPrice != -1;
            bool hasMax = MaxPrice != -1;

            if (!hasMin && !hasMax)
                return products.ToList();

            var result = products;

            if (hasMin)
                result = result.Where(p => p.CostProduct >= MinPrice).ToList();

            if (hasMax)
                result = result.Where(p => p.CostProduct <= MaxPrice).ToList();

            if (hasMin && hasMax && MinPrice > MaxPrice)
                return new List<Product>();

            return result;
        }

        private List<Product> SearchProducts(List<Product> products)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return products.ToList();

            return products
            .Where(p => p.ProductName.ToLower().Contains(SearchText.ToLower()))
            .ToList();

        }


        public void Reset()
        {
            SelectedBrand = null;
            SelectedCountry = null;
            SearchText = "";
            MinPrice = 0;
            MaxPrice = 0;
            _tempBrand = null;
            _tempCountry = null;
            _tempSearch = "";
            _tempMinPrice = 0;
            _tempMaxPrice = 0;
            isCategoryChanged = false;
            isPriceChanged = false;
        }

    }
}
