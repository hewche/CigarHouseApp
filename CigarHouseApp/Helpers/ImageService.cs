using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CigarHouseApp.Helpers
{
    public class ImageService
    {
        private readonly string avatarsDirectory;
        private readonly string productDirectory;


        public ImageService()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            avatarsDirectory = @"..\..\..\ImagesUser\"; ;
            productDirectory = Path.Combine(appDirectory, "ImagesDB");

            //if (!Directory.Exists(avatarsDirectory))
            //    Directory.CreateDirectory(avatarsDirectory);
        }

        public string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите фото";
            openFileDialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileName;

            return null;
        }

        public string UploadPhoto(string folder)
        {

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Выберите фото";
                openFileDialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (openFileDialog.ShowDialog()==true)
                {
                    FileInfo fileInfo = new FileInfo(openFileDialog.FileName);

                    string picFileName = Path.GetFileName(openFileDialog.FileName);
                    string distinPath = @"..\..\..\"+folder +@"\" + picFileName;

                    if (!File.Exists(distinPath))
                    {
                        fileInfo.CopyTo(distinPath);
                    }
                    return picFileName;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки фото: {ex.Message}");
                return null;
            }
        }
    }
}
