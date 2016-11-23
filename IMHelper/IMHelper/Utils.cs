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
using Java.Lang;
using Java.IO;
using Android.Content.PM;
using Android.Graphics;
using Android.Provider;
using System.IO;
using Uri = Android.Net.Uri;


namespace IMHelper
{
    class Utils
    {
        public static bool IsRooted()
        {
            bool retval = false;
            Java.Lang.Process suProcess;
            try
            {
                suProcess = Runtime.GetRuntime().Exec("su");
                var os = new Java.IO.DataOutputStream(suProcess.OutputStream);
                var osRes = new Java.IO.DataInputStream(suProcess.InputStream);
                if (null != os && null != osRes)
                {
                    os.WriteBytes("id\n");
                    os.Flush();
                    string currUid = osRes.ReadLine();
                    bool exitSu = false;
                    if (null == currUid)
                    {
                        retval = false;
                        exitSu = false;
                        System.Console.WriteLine("Can't get root access or denied by user");
                    }
                    else if (true == currUid.Contains("uid=0"))
                    {
                        retval = true;
                        exitSu = true;
                        System.Console.WriteLine("Root access granted");
                    }
                    else
                    {
                        retval = false;
                        exitSu = true;
                        System.Console.WriteLine("Root access rejected: " + currUid);
                    }

                    if (exitSu)
                    {
                        os.WriteBytes("exit\n");
                        os.Flush();
                    }
                }
            }
            catch (Java.Lang.Exception e)
            {
                retval = false;
                System.Console.WriteLine("Root access rejected [" + e.Class.Name + "] : " + e.Message);
            }

            return retval;
        }
        public static string ExecuteShell(string prog, string command)
        {
            Java.Lang.Process p;
                p = Runtime.GetRuntime().Exec(new string[] { prog, "-c", command });
                BufferedReader reader = new BufferedReader(
                  new InputStreamReader(p.InputStream));
                int read;
                char[] buffer = new char[4096];
                StringBuffer output = new StringBuffer();
                while ((read = reader.Read(buffer)) > 0)
                {
                    output.Append(buffer, 0, read);
                }
                reader.Close();

                // Waits for the command to finish.
                p.WaitFor();

                return output.ToString();


            }
        public static string RunRootCommand(string command)
        {
            return ExecuteShell("su", command);
        }
        public static  bool HasSU()
        {
            return new Java.IO.File("/system/app/Superuser.apk").Exists();
        }

        public static void LaunchApp(Context context,string appname)
        {
            //Intent intent = new Intent(Intent.ActionMain);
            //intent.SetComponent(new ComponentName("com.package.address", "com.package.address.MainActivity"));
            //StartActivity(intent);
            PackageManager pm = context.PackageManager;
            Intent intent = pm.GetLaunchIntentForPackage(appname);
            context.StartActivity(intent);
            
        }
        public static void ShareApp(Context context,string appname,string message)
        {
            Intent shareIntent = new Intent(Intent.ActionSend);
            shareIntent.SetType("text/plain");
            shareIntent.PutExtra(Intent.ExtraText, message);
            context.StartActivity(shareIntent);
        }

        public static void ShareText(Context context,string subject,string body)
        {
            Intent sharingIntent = new Intent(Intent.ActionSend);
            sharingIntent.SetType("text/plain");
            sharingIntent.PutExtra(Intent.ExtraSubject, subject);
            sharingIntent.PutExtra(Intent.ExtraText, body);
            context.StartActivity(Intent.CreateChooser(sharingIntent, "Share using"));
        }

        public static void ShareImage(Context context,string title,string filename)
        {
            Bitmap b = BitmapFactory.DecodeFile(filename);
            Intent share = new Intent(Intent.ActionSend);
            share.SetType("image/jpeg");
            MemoryStream bytes = new MemoryStream();
            b.Compress(Bitmap.CompressFormat.Jpeg, 100, bytes);
            string path = MediaStore.Images.Media.InsertImage(context.ContentResolver,
                    b, title, null);
            Uri imageUri = Uri.Parse(path);
            share.PutExtra(Intent.ExtraStream, imageUri);
            context.StartActivity(Intent.CreateChooser(share, "Select"));
        }
        public static void OpenFile(Context context,string filename)
        {
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(Uri.Parse("file:///" + filename), "application/pdf");
            intent.SetFlags(ActivityFlags.ClearTop);
            context.StartActivity(intent);
        }
    }
}