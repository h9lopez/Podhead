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

        static void Main(string[] args)
        {
            var terms = new SearchTerms();
            terms.rawString = "if i were you";

            AppleSearchEngine engine = new AppleSearchEngine();
            List<Series> searchRes = engine.searchSeries(terms);
            Console.WriteLine("Count: " + searchRes.Count);
            if (searchRes.Count <= 0)
            {
                Console.WriteLine("Returned no results");
            }
            Console.WriteLine(searchRes[0]);

            Console.WriteLine("Grabbing first search result and doing full parse");


            RSSReader reader = new RSSReader();
            Series fullSeries = searchRes[0];
            var succ = reader.ParseSeriesRSS(ref fullSeries);
            if (succ == false)
            {
                Console.WriteLine("Could not parse full series RSS feed");
            }

            Console.WriteLine("After parse: " + fullSeries);
            Console.WriteLine("Downloading most recent episode {0}", fullSeries.Episodes[0].Name);

            PodStreamer.DownloadMedia(fullSeries.Episodes[0].ContentInfo.Link.ToString());
        }
        
    }
}
