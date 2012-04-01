using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO;

namespace MobileClient
{
    public class Imager
    {
        public BitmapImage BytesToImage(byte[] array, int width, int height)
        {
            WriteableBitmap result = new WriteableBitmap(width, height);
            MemoryStream ms = new MemoryStream();
            ms.Write(array, 0, array.Length);
            result.SetSource(ms);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(ms);
            return bitmapImage;
        }

        public byte[] ImageToBytes(BitmapImage bitmapImage)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                WriteableBitmap writableBitmap = new WriteableBitmap(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
                Extensions.SaveJpeg(writableBitmap, ms, bitmapImage.PixelWidth, bitmapImage.PixelHeight, 0, 100);
                return ms.ToArray();
            }
        }
    }
}
