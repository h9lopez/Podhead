using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PodcastGrabber
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    RSSReader reader = new RSSReader( new Uri("C:\\Users\\Huandari\\Desktop\\radiolab.xml") );
        //    //reader.RetrieveRSS();
        //    reader.rssData = "C:\\Users\\Huandari\\Desktop\\radiolab.xml";
        //    reader.ParseRSS();
        //}

        static void Main(string[] args)
        {
            WebRequest req = WebRequest.Create("https://itunes.apple.com/search?term=fighter+and+the+kid&limit=1&media=podcast&entity=podcast");
            req.Method = WebRequestMethods.Http.Get;
            req.ContentType = "application/json";
            var res = (HttpWebResponse)req.GetResponse();
            string ptext;

            using (var sr = new StreamReader(res.GetResponseStream()))
            {
                ptext = sr.ReadToEnd();
            }

            Console.WriteLine(ptext);

            AppleRepoParser parser = new AppleRepoParser();
            var results = parser.ParseJSON(ptext);

            Console.WriteLine(results.Count);

            Console.WriteLine(results[0].FeedLink);
            RSSReader reader = new RSSReader( new Uri(results[0].FeedLink) );
            //reader.rssData = GetRSSFeed( results[0].FeedLink );
            reader.rssData = results[0].FeedLink;
            reader.ParseRSS();

            res.Close();
        }

        public static string GetRSSFeed(string url)
        {
            WebRequest req = WebRequest.Create(url);
            req.Method = WebRequestMethods.Http.Get;
            req.ContentType = "application/rss+xml";
            var res = (HttpWebResponse)req.GetResponse();
            string resStr;

            using (var sr = new StreamReader(res.GetResponseStream()))
            {
                resStr = sr.ReadToEnd();
            }

            res.Close();
            return resStr;
        }
    }
}
