using System;
using System.Windows.Media.Imaging;
using System.IO;

namespace DesktopClient
{
    public class StandartPS: IPStorage
    {
        private string _path;

        public StandartPS(string path)
        {
            _path = path;
        }

        public void Save(BitmapSource bmSource)
        {
            BitmapEncoder encoder;
            switch (Path.GetExtension(_path).ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    encoder = new JpegBitmapEncoder();
                    break;
                case ".gif":
                    encoder = new GifBitmapEncoder();
                    break;
                case ".bmp":
                    encoder = new BmpBitmapEncoder();
                    break;
                case ".tif":
                case ".tiff":
                    encoder = new TiffBitmapEncoder();
                    break;
                case ".png":
                    encoder = new PngBitmapEncoder();
                    break;
                default:
                    throw new ArgumentException("Wrong path");
            }
            encoder.Frames.Add(BitmapFrame.Create(bmSource));
            Stream stm = File.Create(_path);
            encoder.Save(stm);
            stm.Dispose();
        }

        public BitmapSource Load()
        {
            BitmapImage bmImage = new BitmapImage();
            bmImage.BeginInit();
            bmImage.UriSource = new Uri(_path, UriKind.Relative);
            bmImage.CacheOption = BitmapCacheOption.OnLoad;
            bmImage.EndInit();
            return (BitmapSource)bmImage;
        }
    }
}
