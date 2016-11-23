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
using Android.Locations;
using System.Threading.Tasks;

namespace IMHelper
{

    public class IMLocation
    {
        public static LocationManager _locationManager;
        static string _locationProvider;
        Location _currentLocation;

        public static async Task<Address> ReverseGeocodeCurrentLocation(Context context, Location _currentLocation)
        {
            Geocoder geocoder = new Geocoder(context);
            IList<Address> addressList =
                await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

            Address address = addressList.FirstOrDefault();
            return address;
        }

        public class IMLocationProvider : Java.Lang.Object, ILocationListener
        {
            public LocationManager locMgr;
            public Location _Location;
            public event EventHandler LocationChanged;
            public event EventHandler LocationStatusChanged;


            public IMLocationProvider(Context context)
            {
                locMgr = context.GetSystemService(Context.LocationService) as LocationManager;
                if (locMgr.AllProviders.Contains(LocationManager.NetworkProvider)
                    && locMgr.IsProviderEnabled(LocationManager.NetworkProvider))
                {
                    locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, 2000, 1, this);
                }
                else
                {
                    Toast.MakeText(context, "The Network Provider does not exist or is not enabled!", ToastLength.Long).Show();
                }





            }
            public void OnLocationChanged(Location location)
            {


                _Location = location;
                if (LocationChanged != null)
                {
                    LocationChanged(this, new AndroidLocationChangedEventArgs(true, _Location));
                }


            }

            public void OnProviderDisabled(string provider)
            {
                if (LocationStatusChanged != null)
                {
                    LocationStatusChanged(this, new AndroidLocationChangedEventArgs(false, null));
                }
            }

            public void OnProviderEnabled(string provider)
            {
                if (LocationStatusChanged != null)
                {
                    LocationStatusChanged(this, new AndroidLocationChangedEventArgs(true, _Location));
                }
            }

            public void OnStatusChanged(string provider, Availability status, Bundle extras)
            {
                if (LocationStatusChanged != null)
                {
                    var hasFix = status == Availability.Available;
                    LocationStatusChanged(this, new AndroidLocationChangedEventArgs(hasFix, _Location));
                }
            }
        }

        public class AndroidLocationChangedEventArgs : EventArgs
        {


            public AndroidLocationChangedEventArgs(bool hasFix, Location location)
            {
                HasFix = hasFix;
                Location = location;


            }

            public Location Location { get; set; }
            public bool HasFix { get; set; }
        }
    }

}