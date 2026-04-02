using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CigarHouseApp.Helpers
{
    public class HeartIconConverter
    {

        public void ToggleHeartColor(Button heart)
        {
            if (heart.Tag.ToString()=="NonSelected")
            {
                heart.Tag = "Selected";
            }
            else
            {
                heart.Tag = "NonSelected";
            }
        }
    }
}
