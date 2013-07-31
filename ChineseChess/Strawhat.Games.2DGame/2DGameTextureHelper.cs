using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;

namespace Strawhat.Games._2DGame
{
    public static class _2DGameTextureHelper
    {
        public static BitmapImage LoadBitmapImageToMemory(string filePath)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = new MemoryStream(File.ReadAllBytes(filePath));
            image.EndInit();
            image.Freeze();
            return image;
        }
    }
}
