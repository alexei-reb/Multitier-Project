using System;
using System.Windows.Media.Imaging;

namespace DesktopClient
{
    public interface IPStorage
    {
        void Save(BitmapSource bmSource);
        BitmapSource Load();
    }
}
