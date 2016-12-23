using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace PodcastGrabber
{
    class PodStreamer
    {
        public static void GetData(string uri)
        {
            WebRequest req = WebRequest.Create(uri);
            req.Method = WebRequestMethods.Http.Get;
            //req.ContentType = "application/mpeg";
            DateTime beg = DateTime.Now;

            var res = (HttpWebResponse)req.GetResponse();

            using (var sr = new StreamReader(res.GetResponseStream()))
            {
                var resStr = sr.ReadToEnd();
                Console.WriteLine("Length: " + resStr.Length);
            }
            DateTime end = DateTime.Now;

            Console.WriteLine("Took {0} seconds", end - beg);
        }
    }
}
