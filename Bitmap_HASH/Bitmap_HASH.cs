using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Bitmap_HASH
{
    public class Bitmap_HASH
    {
        public static ulong GetHASH(Bitmap bitmap, string mode = "high", int bitmode = 64)
        {
            int mHeight;
            int mWidth;
            switch (mode)
            {
                case "low":
                    mHeight = 4;
                    mWidth = 4;
                    break;
                case "normal":
                    mHeight = 4;
                    mWidth = 8;
                    break;
                case "high":
                    mHeight = 8;
                    mWidth = 8;
                    break;
                default:
                    throw new Exception("Invalid mode! Please choose the mode between 'high', 'normal' or 'low'!");
            }
            Bitmap mBitmap = ResizeBitmap(bitmap, mWidth, mHeight);
            byte[] array = BitmapToByteArray(mBitmap);
            byte[] bwarray = BytesRGBtoBlackWhite(array);
            byte[] twoarray = AveragingSimplification(bwarray);
            for (int i = 0; i < twoarray.Length; i++)
                Console.Write(" " + twoarray[i]);
            Console.WriteLine();
            switch (bitmode)
            {
                case 32:
                    return BitConverter.ToUInt32(twoarray, 0);
                case 64:
                    return BitConverter.ToUInt64(twoarray, 0);
                default:
                    throw new Exception("Invalid bitmode! Please choose the mode between '64', '32'!");
            }
        }

        public static int HammingDistance(ulong a, ulong b)
        {
            var astr = a.ToString();
            var bstr = b.ToString();
            int len = Math.Min(astr.Length, bstr.Length);
            int result = 0;
            
            for (int i = 0; i < len; i++)
            {
                if (astr[i] != bstr[i])
                    result++;
            }
            return result;

        }

        private static byte[] AveragingSimplification(byte[] bytes)
        {
            int average = 0;
            int len = bytes.Length;
            byte[] result = new byte[len];
            for (int i = 0; i < len; i++)
            {
                average += bytes[i];
            }
            average = average / len;
            for (int i = 0; i < len; i++)
            {
                if (bytes[i] < average)
                    result[i] = 0;
                else
                    result[i] = 1;
            }
            return result;
        }

        private static byte[] BytesRGBtoBlackWhite(byte[] rgb)
        {
            int len = rgb.Length;
            byte[] result = new byte[len / 3];
            for (int i = 0; i < len; i += 3)
            {
                result[i / 3] = (byte)((rgb[i] + rgb[i + 1] + rgb[i + 2]) / 3);
            }
            return result;
        }

        private static byte[] BitmapToByteArray(Bitmap bitmap)
        {

            BitmapData bmpdata = null;

            try
            {
                bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                int numbytes = bmpdata.Stride * bitmap.Height;
                byte[] bytedata = new byte[numbytes];
                IntPtr ptr = bmpdata.Scan0;

                Marshal.Copy(ptr, bytedata, 0, numbytes);

                return bytedata;
            }
            finally
            {
                if (bmpdata != null)
                    bitmap.UnlockBits(bmpdata);
            }

        }


        private static Bitmap ResizeBitmap(Bitmap bitmap, int newWidth, int newHeight)
        {
            Bitmap b = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(bitmap, 0, 0, newWidth, newHeight);
            g.Dispose();

            return b;
        }
    }
}
