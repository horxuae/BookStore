using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace BookStore.CommonProject
{
    /// <summary>
    /// 圖檔共用方法
    /// <summary>
    public class ImageTransfer
    {
        // Varbinary => Image
        public Image ConvertToImage(System.Data.Linq.Binary linqBinary)
        {
            byte[] picBinary = linqBinary.ToArray();
            Image image = null;

            using (MemoryStream ms = new MemoryStream(picBinary))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }

        // Image => Byte[]
        public byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

    }
}