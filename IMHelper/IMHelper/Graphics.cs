using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Content.Res;

namespace IMHelper
{
   public static class Graphics
    {
        public static Bitmap GetImage(Context context, string imagename, string dir)
        {
            Bitmap bmp = null;
            switch (dir)
            {
                case "assets":

                    AssetManager assetManager = context.Assets;
                    System.IO.Stream ims = assetManager.Open(imagename);
                    bmp = BitmapFactory.DecodeStream(ims);
                    break;
                case "sdcard":
                    var sdpath = Android.OS.Environment.ExternalStorageDirectory.Path;
                    string imagepath = System.IO.Path.Combine(sdpath, imagename);
                    if (System.IO.File.Exists(imagepath))
                    {
                        bmp = BitmapFactory.DecodeFile(new Java.IO.File(imagepath).AbsolutePath);
                    }
                    break;
                case "internal":
                    var sdpaths = Android.OS.Environment.DataDirectory.Path;
                    string imagepaths = System.IO.Path.Combine(sdpaths, imagename);
                    if (System.IO.File.Exists(imagepaths))
                    {
                        bmp = BitmapFactory.DecodeFile(new Java.IO.File(imagepaths).AbsolutePath);
                    }
                    break;
            }
            return bmp;
        }
        public static Bitmap Bytes2Bitmap(byte[] data)
        {
            return BitmapFactory.DecodeByteArray(data, 0, data.Length);
        }
        public static Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            var bitmapScalled = Bitmap.CreateScaledBitmap(bmp, width, height, true);
            bmp.Recycle();
            return bitmapScalled;
        }
    }
}