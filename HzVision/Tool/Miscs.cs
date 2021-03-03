using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Drawing.Imaging;


namespace Vision.Misc
{
    public partial class Miscs
    {
        public static void AdjHalconWindow(Control parent, HWindowControl hWindow, int imgWidth, int imgHeight)
        {
            if (imgWidth < 10 || imgHeight < 10) return;

            int nw = parent.ClientSize.Width;
            int nh = parent.ClientSize.Height;
            if (nh > nw * imgHeight / imgWidth)
            {
                nh = (nw * imgHeight / imgWidth);
            }
            else
            {
                nw = (nh * imgWidth / imgHeight);
            }
            hWindow.Width = nw;
            hWindow.Height = nh;
            hWindow.Top = (parent.ClientSize.Height - nh) / 2;
            hWindow.Left = (parent.ClientSize.Width - nw) / 2;
        }


        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", CharSet = CharSet.Auto)]
        private extern static long CopyMemory(IntPtr dest, IntPtr source, int size);

        public static bool ConvertGrayBitmap(HImage image, out Bitmap bitmap)
        {
            bitmap = null;
            if (image == null || !image.IsInitialized())
            {
                return false;
            }

            string type;
            int width, height;
            IntPtr ptr_Source = image.GetImagePointer1(out type, out width, out height);
            if (type != "byte")
            {
                return false;
            }

            // 设置调色板
            bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette pal = bitmap.Palette;
            for (int i = 0; i <= 255; i++)
            {
                pal.Entries[i] = Color.FromArgb(255, i, i, i);
            }
            bitmap.Palette = pal;

            // 获取图片数据指针
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            int PixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
            int stride = bitmapData.Stride;
            bitmap.UnlockBits(bitmapData);

            IntPtr[] ptr = new IntPtr[2];
            ptr[0] = bitmapData.Scan0;
            ptr[1] = ptr_Source;

            // 图片数据内存拷贝
            if (width % 4 == 0)
            {
                CopyMemory(ptr[0], ptr[1], width * height * PixelSize);
            }
            else
            {
                for (int i = 0; i < height - 1; i++)
                {
                    ptr[1] += width;
                    CopyMemory(ptr[0], ptr[1], width * PixelSize);
                    ptr[0] += stride;
                }
            }

            return true;
        }

        //private bool ConvertRGBBitmap(HImage image, out Bitmap bitmap)
        //{
        //    bitmap = null;
        //    if (image == null || !image.IsInitialized())
        //    {
        //        return false;
        //    }

        //    HTuple hred, hgreen, hblue, type, width, height;
        //    HOperatorSet.GetImagePointer3(image, out hred, out hgreen, out hblue, out type, out width, out height);
        //    if (type != "byte")
        //    {
        //        return false;
        //    }

        //    bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        //    Rectangle rect = new Rectangle(0, 0, width, height);
        //    BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        //    unsafe
        //    {
        //        byte* bptr = (byte*)bitmapData.Scan0;
        //        byte* r = ((byte*)hred.I);
        //        byte* g = ((byte*)hgreen.I);
        //        byte* b = ((byte*)hblue.I);
        //        ParallelLoopResult result = Parallel.For(0, width * height, (i) =>
        //        {
        //            bptr[i * 4 + 0] = (b)[i];
        //            bptr[i * 4 + 1] = (g)[i];
        //            bptr[i * 4 + 2] = (r)[i];
        //            bptr[i * 4 + 3] = 255;
        //        });
        //    }
        //    bitmap.UnlockBits(bitmapData);

        //    return true;
        //}


        public static void HImageToBitmap(HImage image, out Bitmap bitmap)
        {
            bitmap = null;
            if (!image.IsInitialized())
            {
                return ;
            }

            if (image.CountChannels() == 1)
            {
                ConvertGrayBitmap(image, out bitmap);
            }
            else if (image.CountChannels() == 3)
            {
                //ConvertRGBBitmap(image, out bitmap);
                throw new Exception("不支持的图像格式");
            }
            else
            {
                throw new Exception("不支持的图像格式");
            }
        }



    }



}
