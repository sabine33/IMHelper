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
using Android.Content.Res;
using System.IO;

namespace IMHelper
{
    class IO
    {
        public static string ReadFile(Context context, string filename, string dir)
        {
            string content = "";
            switch (dir)
            {
                case "assets":
                    AssetManager assets = context.Assets;
                    using (StreamReader sr = new StreamReader(assets.Open("read_asset.txt")))
                    {
                        content = sr.ReadToEnd();
                    }
                    break;
                case "sdcard":
                    var sdpath = Android.OS.Environment.ExternalStorageDirectory.Path;
                    string filepath = System.IO.Path.Combine(sdpath, filename);
                    content = System.IO.File.ReadAllText(content);
                    break;
                case "internal":
                    sdpath = Android.OS.Environment.DataDirectory.Path;
                    filepath = System.IO.Path.Combine(sdpath, filename);
                    content = System.IO.File.ReadAllText(content);
                    break;
            }
            return content;
        }


        public static void WriteFile(string filename,byte[] data)
        {
            string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            string name = Path.Combine(path, filename);

            File.WriteAllBytes(name, data);
            
       

        }
    }
}