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
using System.Net;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace IMHelper
{
    public static class Web
    {
        public static byte[] SendGet(string url, string data)
        {

            WebClient wc = new WebClient();
            return wc.DownloadData(url + "?" + data);

        }
        public static string SendPost(string url, NameValueCollection data)
        {
            using (WebClient client = new WebClient())
            {
                byte[] responsebytes = client.UploadValues(url, "POST", data);
                string responsebody = System.Text.Encoding.UTF8.GetString(responsebytes);
                return responsebody;
            }
        }

        public static async Task<int> DownloadFile(string urlToDownload, IProgress<DownloadBytesProgress> progessReporter)
        {
            int receivedBytes = 0;
            int totalBytes = 0;
            WebClient client = new WebClient();
            using (var stream = await client.OpenReadTaskAsync(urlToDownload))
            {
                byte[] buffer = new byte[4096];
                totalBytes = Int32.Parse(client.ResponseHeaders[HttpResponseHeader.ContentLength]);

                for (;;)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        await Task.Yield();
                        break;
                    }

                    receivedBytes += bytesRead;
                    if (progessReporter != null)
                    {
                        DownloadBytesProgress args = new DownloadBytesProgress(urlToDownload, receivedBytes, totalBytes);
                        progessReporter.Report(args);
                    }
                }
            }
            return receivedBytes;
        }


    }
    public class DownloadBytesProgress
    {
        public DownloadBytesProgress(string fileName, int bytesReceived, int totalBytes)
        {
            Filename = fileName;
            BytesReceived = bytesReceived;
            TotalBytes = totalBytes;
        }

        public int TotalBytes { get; private set; }

        public int BytesReceived { get; private set; }

        public float PercentComplete { get { return (float)BytesReceived / TotalBytes; } }

        public string Filename { get; private set; }

        public bool IsFinished { get { return BytesReceived == TotalBytes; } }
    }

}