using CigarHouseApp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace CigarHouseApp.Helpers
{
    internal class ProductConverter : IValueConverter
    {
        public object Convert(object value, Type targerType, object source, CultureInfo cultureInfo)
        {
            if(value is Product product)
            {
                if (product.Cigar != null && !string.IsNullOrEmpty(product.Cigar.Strength))
                {
                    return product.Cigar.Strength;
                }
                if (product.Accessory != null && !string.IsNullOrEmpty(product.Accessory.Color))
                {
                    return product.Accessory.Color;
                }
            }
            return "Не указано";
        }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
    }
}
