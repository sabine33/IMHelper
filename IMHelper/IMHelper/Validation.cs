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
using Java.Util.Regex;
using System.Text.RegularExpressions;

namespace IMHelper
{
    class Validation
    {
        public static  bool IsValidEmail(String email)
        {
            String EMAIL_PATTERN = "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@"
                    + "[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";

            Regex regex = new Regex(EMAIL_PATTERN);
           return  regex.IsMatch(email);
        }

        // validating password with retype password
        public static  bool IsValidField(params string[] fields)
        {
            bool passed = true;
            foreach (var pass in fields)
            {
                if (pass == null || pass.Length < 6)
                {
                    passed = false;
                }
                
            }
            return passed;
        }
    }
}