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
using Android.Preferences;

namespace IMHelper
{
    class Settings
    {
        public static void SavePrefs(Context context, string valueKey, String value)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(valueKey, value);
            editor.Apply();
        }

        public static String ReadPrefs(Context context, String valueKey, String valueDefault)
        {
            ISharedPreferences prefs = PreferenceManager
                .GetDefaultSharedPreferences(context);
            return prefs.GetString(valueKey, valueDefault);
        }

    }
}