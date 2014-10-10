using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace MoveTicketBarcodes
{
    public static class MyExtensionMethods
    {

        public static BitmapImage ToBitmapImage(this System.Drawing.Image source)
        {
            BitmapImage bi = new BitmapImage();

            bi.BeginInit();

            MemoryStream ms = new MemoryStream();

            // Save to a memory stream...
            source.Save(ms, ImageFormat.Bmp);

            // Rewind the stream... 
            ms.Seek(0, SeekOrigin.Begin);


            // Tell the WPF image to use this stream... 
            bi.StreamSource = ms;

            bi.EndInit();

            return bi;
        }
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true;
            else
                return !source.Any();
        }
    }


}
