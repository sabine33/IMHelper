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
using System.Timers;
using Android.Graphics;
using Android.Media;
using Android.Content.Res;
using Android.Speech;
using Android.Speech.Tts;
using Java.Util;

namespace IMHelper
{
    class Multimedia
    {
        public static MediaRecorder _recorder;
        public static void PlayMusic(Context context, string dir, string file, SeekBar seekbar)
        {

            MediaPlayer mp = new MediaPlayer();

            switch (dir)
            {
                case "assets":
                    AssetFileDescriptor descriptor = context.Assets.OpenFd(file);
                    mp.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
                    descriptor.Close();
                    break;
                case "external":
                    Java.IO.File filefd = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, file);
                    mp.SetDataSource(context, Android.Net.Uri.FromFile(filefd));
                    break;

            }
            mp.Prepare();
            mp.SetVolume(1f, 1f);
            mp.Looping = (true);
            mp.Start();
            seekbar.IncrementSecondaryProgressBy(1);
            seekbar.SecondaryProgressTintMode = PorterDuff.Mode.Lighten;
            seekbar.Max = mp.Duration;
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Start();

            timer.Elapsed += delegate
            {
                // seekbar.SecondaryProgress = mp.CurrentPosition;
                seekbar.Progress = mp.CurrentPosition;

            };

            mp.Completion += delegate
            {
                timer.Stop();
                mp.Stop();
            };
            //seekbar.ProgressChanged += (s,e)=>
            //{

            //    mp.SeekTo(e.SeekBar.SecondaryProgress);
            //};

        }


        public static void RecognizeVoice(Activity context,string message,int id)
        {
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, message);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            context.StartActivityForResult(voiceIntent, id);

            /* var matches = data.GetStringArrayListExtra (RecognizerIntent.ExtraResults);
            string textInput = text.Text + matches [0].ToString ();
            if (textInput.Length > 500)
                textInput = textInput.Substring (0, 500);
            ProcessText(textInput);
            */
        }

        public static void RecordVoice(string path)
        {
            _recorder = new MediaRecorder();
            _recorder.SetAudioSource(AudioSource.Default);
            _recorder.SetOutputFormat(OutputFormat.Default);
            _recorder.SetAudioEncoder(AudioEncoder.Default);
            _recorder.SetOutputFile(path);
            _recorder.Prepare();
            _recorder.Start();
         
        }

        public static void StopRecording()
        {
            if (_recorder != null)
            {
                _recorder.Stop();
                _recorder.Release();
                _recorder.Dispose();
            }
        }
        public static void PlayTTS(Context context,string text,float rate,bool queueflush)
        {
            TextToSpeech tts = new TextToSpeech(context, null, "com.google.android.tts");
            tts.SetSpeechRate(rate);
            tts.SetLanguage(Locale.Default);
            QueueMode mode = queueflush ? QueueMode.Flush : QueueMode.Add;
            tts.Speak(text, mode,null,null);
        }


    }
}
 


