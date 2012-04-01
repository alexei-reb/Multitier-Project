using System;
using System.IO;

namespace DesktopClient
{
    public static class PSFactory
    {
        public static IPStorage GetInstance(string path)
        {
            switch (Path.GetExtension(path).ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".png":
                case ".bmp":
                case ".tif":
                case ".tiff":
                    return new StandartPS(path);
                case ".psd":
                    return new PhotoshopPS(path);
                default:
                    throw new ArgumentException("Unknown file format");
            }
        }
    }
}
