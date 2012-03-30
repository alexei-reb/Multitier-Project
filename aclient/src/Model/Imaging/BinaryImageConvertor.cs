using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;

namespace DesktopClient
{
    public class BinaryImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(Byte[]))
                return null;

            byte[] binaryData = (byte[])value;
            var bmp = new System.Windows.Media.Imaging.BitmapImage();

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(binaryData))
            {
                bmp.BeginInit();
                bmp.StreamSource = stream;
                bmp.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bmp.EndInit();
            }

            if (bmp.CanFreeze)
                bmp.Freeze();
            return bmp;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(value as BitmapSource));
            encoder.Save(memStream);
            return memStream.GetBuffer();
        }
    }
}
