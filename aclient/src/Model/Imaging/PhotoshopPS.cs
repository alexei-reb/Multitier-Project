using System;
using System.Windows.Media.Imaging;
using System.Windows;

namespace DesktopClient
{
    public class PhotoshopPS: IPStorage
    {
        private string _path;

        public PhotoshopPS(string path)
        {
            _path = path;
        }

        public void Save(BitmapSource bmSource)
        {
            BitmapEncoder encoder = new TiffBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmSource));
            System.IO.Stream strm = new System.IO.MemoryStream();
            encoder.Save(strm);
            strm.Position = 0;
            Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(strm);
            image.Save(_path, new Aspose.Imaging.SaveOptions.PsdSaveOptions());
        }

        public BitmapSource Load()
        {
            SimplePsd.CPSD psd = new SimplePsd.CPSD();
            psd.Load(_path);
            BitmapSource bmSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                psd.GetHBitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            return bmSource;
        }
    }
}
