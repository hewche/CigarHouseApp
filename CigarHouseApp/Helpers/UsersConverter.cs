using CigarHouseApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CigarHouseApp.Helpers
{
    public class UsersConverter : IValueConverter
    {
        public object Convert(object value, Type targerType, object source, CultureInfo cultureInfo)
        {
            if (value is User user)
            {
                if(user.RoleId == 1 || user.RoleId == 3) 
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
