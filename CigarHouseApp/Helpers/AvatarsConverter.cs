using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CigarHouseApp.Helpers
{
    public class AvatarsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (value is string fileName && !string.IsNullOrEmpty(fileName))
            {
                try
                {
                    string imagePath = path+ @"..\..\..\ImagesUser\"+fileName;

                    var uri = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                    var bitmap = new BitmapImage(uri);
                    return bitmap;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка загрузки {fileName}: {ex.Message}");
                }
            }
            try
            {
                string placeholderPath = path+ @"..\..\..\Images\Image_not_available.png";

                var uri = new Uri(placeholderPath, UriKind.RelativeOrAbsolute);
                var bitmap = new BitmapImage(uri);
                return bitmap;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}