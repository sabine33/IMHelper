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
using Android.Hardware;

namespace IMHelper
{
    class Sensors : Java.Lang.Object,ISensorEventListener
    {

        Action<string> onsensorchanged;
        Action<string> onAccuracyChanged;
        Context context;
        SensorManager _senMan;
        SensorType type;
        bool hasUpdated = false;
        DateTime lastUpdate;
        float last_x = 0.0f;
        float last_y = 0.0f;
        float last_z = 0.0f;
        public Action<string> shaken;
         int ShakeDetectionTimeLapse = 250;
         double ShakeThreshold = 800;
        public Sensors(Context context,SensorType type,Action<string> onsensorchanged,Action<string> acc)
        {
            this.context = context;
            this.onsensorchanged = onsensorchanged;
            this.onAccuracyChanged = acc;
            this.type = type;
            _senMan = (SensorManager)context.GetSystemService(Context.SensorService);
            Sensor sen = _senMan.GetDefaultSensor(type);
            _senMan.RegisterListener(this, sen, Android.Hardware.SensorDelay.Normal);
        }
        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
         
        }

        public void DetectShake(Action<string> shaken)
        {
            this.shaken = shaken;

        }
       
        public void OnSensorChanged(SensorEvent e)
        {
            switch(type)
            {
                case SensorType.Light:
                    onsensorchanged(e.Values[0].ToString("0.00"));
                    break;
                case SensorType.Accelerometer:
                    float x = e.Values[0];
                    float y = e.Values[1];
                    float z = e.Values[2];
                    onsensorchanged(x.ToString() + "," + y.ToString() + "," + z.ToString());
                    DateTime curTime = System.DateTime.Now;
                    if (hasUpdated == false)
                    {
                        hasUpdated = true;
                        lastUpdate = curTime;
                        last_x = x;
                        last_y = y;
                        last_z = z;
                    }
                    else
                    {
                        if ((curTime - lastUpdate).TotalMilliseconds > ShakeDetectionTimeLapse)
                        {
                            float diffTime = (float)(curTime - lastUpdate).TotalMilliseconds;
                            lastUpdate = curTime;
                            float total = x + y + z - last_x - last_y - last_z;
                            float speed = Math.Abs(total) / diffTime * 10000;

                            if (speed > ShakeThreshold)
                            {
                               if(shaken!=null) shaken(speed.ToString());  
                            }

                            last_x = x;
                            last_y = y;
                            last_z = z;
                        }
                    
            }
                    break;
                case SensorType.Pressure:
                    var hPAs = e.Values[0];
                    var calcedAltitude = calculateAltitudeInFeet(hPAs);
                    onsensorchanged(hPAs + "," + calcedAltitude.ToString());
                    break;
            }
          
        }
        public void Stop()
        {
            _senMan.UnregisterListener(this);
        }

        double calculateAltitudeInFeet(float hPAs)
        {
            var pstd = 1013.25;
            var altpress = (1 - Math.Pow((hPAs / pstd), 0.190284)) * 145366.45;
            return altpress;
        }
    }
}