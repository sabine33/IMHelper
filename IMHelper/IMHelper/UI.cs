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
using Android;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace IMHelper
{
   public class UI
    {
        public static void ShowToast(Context context, string message, bool isLong)
        {

            ToastLength len = isLong ? ToastLength.Long : ToastLength.Short;
            Toast.MakeText(context, message, len).Show();
        }
        public static void MsgBox(Context context,string title,string message,string positivebtn,Action posaction,string negativebtn,Action negaction)
        {
            Dialog dialog = null;
            AlertDialog.Builder alert = new AlertDialog.Builder(context);
            alert.SetTitle(title);
            alert.SetMessage(message);
            if (positivebtn != null)
            {
                alert.SetPositiveButton(positivebtn, (senderAlert, args) =>
                {
                    if (posaction != null) posaction();
                    dialog.Hide();
                });
            }
            if (negativebtn != null)
            {
                alert.SetNegativeButton(negativebtn, (senderAlert, args) =>
                {
                    if (negaction != null) negaction();
                    dialog.Hide();
                });
            }
            dialog = alert.Create();
            
            dialog.Show();
           
        }
        public static void PopulateListView(Context context, ListView lv, string[] items)
        {

            lv.Adapter = new ArrayAdapter<string>(context,
                                                  Android.Resource.Layout.SimpleListItem1, items);
        }

        public static void ChooseImage(Activity activity, int id)
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            activity.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), id);
        }

        public static void PopulateSpinner(Context context, Spinner spinner, string[] data)
        {
            var adapter = new ArrayAdapter(context, Android.Resource.Layout.SimpleSpinnerItem, data);
            spinner.Adapter = adapter;
            //spinner.ItemSelected += delegate (object sender, Spinner.ItemSelectedEventArgs e)
            //  {
            //      toast(context, "clicked "+spinner.SelectedItem.ToString(), false);
            //  };

        }
        public static void PopulateListView(Context context, ListView listview,int resource, string[] header, int[] image, string[] info, int[] to)
        {
            var aList = new JavaList<IDictionary<string, object>>();

            for (int i = 0; i < header.Length; i++)
            {
                var hm = new JavaDictionary<string, object>();
                hm.Add("header", header[i].ToString());
                hm.Add("info", info[i].ToString());
                hm.Add("image", image[i].ToString());
                aList.Add(hm);
            }
            //to=id
            // Keys used in Hashmap
            String[] from = { "header", "info", "image" };
            SimpleAdapter adapter = new SimpleAdapter(context, aList,resource, from, to);

            listview.Adapter = adapter;

        }
        public static void ShowNotification(Context context,Type afterclick,string value2pass,string title,string message,int icon,int id)
        {

            // When the user clicks the notification, SecondActivity will start up.
            Intent resultIntent = new Intent(context, afterclick);

            // Pass some values to SecondActivity:
            resultIntent.PutExtra("data", value2pass);

            // Construct a back stack for cross-task navigation:
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(context);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(afterclick));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            PendingIntent resultPendingIntent =
                stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            // Build the notification:
            Notification.Builder builder = new Notification.Builder(context)
                .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                .SetContentIntent(resultPendingIntent)  // Start 2nd activity when the intent is clicked.
                .SetContentTitle(title)     // Set its title
                .SetSmallIcon(icon>0 ? icon : Resource.Drawable.IcDialogAlert) // Display this icon
                .SetContentText(message); // The message to display.

            // Finally, publish the notification:
            NotificationManager notificationManager =
                (NotificationManager)context.GetSystemService(Context.NotificationService);
            notificationManager.Notify(id, builder.Build());
            
        }



        //public static FloatingActionButton AddFab(Context context,Bitmap icon)
        //{
        //    FloatingActionButton fab = new FloatingActionButton(context);
        //    fab.SetForegroundGravity(GravityFlags.Bottom | GravityFlags.Right);
        //    fab.SetImageIcon(Icon.CreateWithBitmap(icon));
        //    fab.SetRippleColor(Color.BlueViolet);
        //    return fab;
        //}

        public static void ShowChooserDialog(Context context,string[] options,Action<int> action)
        {
           

            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle("Pick a color");
            builder.SetItems(options, (sender, e) =>
             {
                 action(e.Which);
             });
            builder.Show();
        }

    }
}