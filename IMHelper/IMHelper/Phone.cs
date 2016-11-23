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
using Android.Telephony;
using Android.Provider;

namespace IMHelper
{
    class Phone
    {
        public static void MakeCall(Context context, string number)
        {
            var uri = Android.Net.Uri.Parse("tel:" + number);
            var intent = new Intent(Intent.ActionDial, uri);
            context.StartActivity(intent);
        }

        public static void SendSMS(string receiver, string message)
        {
            SmsManager.Default.SendTextMessage(receiver, null, message, null, null);

        }
        public static void SendSMS(Context context, string receiver, string message)
        {
            var smsUri = Android.Net.Uri.Parse("smsto:" + receiver);
            var smsIntent = new Intent(Intent.ActionSendto, smsUri);
            smsIntent.PutExtra("sms_body", message);
            context.StartActivity(smsIntent);
        }

        public static void CaptureImage(Activity activity, int id)
        {

            Intent cameraIntent = new Intent(MediaStore.ActionImageCapture);
            activity.StartActivityForResult(cameraIntent, id);

        }
        public static void CaptureImage(Activity activity, string targetdir, string targetImagename, int id)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            Java.IO.File file = new Java.IO.File(targetdir, String.Format(targetImagename + "_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(file));
            activity.Intent.PutExtra("path", file.AbsolutePath);
            activity.StartActivityForResult(intent, id);

        }
    }
}