using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace PodcastGrabber
{
    class RSSReader
    {
        private Uri feedUri;
        public string rssData;
        private XmlNamespaceManager nsm;

        public RSSReader(Uri feedUri)
        {
            this.feedUri = feedUri;
        }

        public Series ParseSeriesRSS()
        {
            
            XmlDocument doc = new XmlDocument();
            Series pSeries = new Series();

            try
            {
                doc.Load(this.rssData);
            } catch (Exception)
            {
                Console.WriteLine("Error loading from XML stream.");
            }

            this.nsm = new XmlNamespaceManager(doc.NameTable);
            nsm.AddNamespace("itunes", "http://www.itunes.com/dtds/podcast-1.0.dtd");

            // Extract info from the podcast series itself
            XmlNode root = doc.SelectSingleNode("rss/channel");
            Console.WriteLine(root.Name + ", " + root.Value);

            XmlNode seriesName = root.SelectSingleNode("title");
            pSeries.Name = seriesName.InnerText;
            XmlNode seriesDesc = root.SelectSingleNode("description");
            pSeries.Description = seriesDesc.InnerText;
            XmlNode seriesAuthor = root.SelectSingleNode("itunes:author", this.nsm);
            pSeries.Author = seriesAuthor.InnerText;
            pSeries.FeedLink = this.rssData;

            XmlNodeList eps = root.SelectNodes("item");
            Console.WriteLine("Length: " + eps.Count);

            var parsedEps = this.parseEpisodes(eps);
            Console.WriteLine(parsedEps[0]);
            //foreach (Episode ep in parsedEps)
            //{
            //    Console.WriteLine(ep + "\n==================");
            //}

            pSeries.Episodes = parsedEps;
            return pSeries;

        }

        public Episode parseEpisode(XmlNode ep)
        {
            Episode nEp = new Episode();
            DateTime timeTry;
            XmlNode cLink;

            nEp.Name                    = this.grabTagText(ep, "title");
            nEp.Link                    = new Uri(this.grabTagText(ep, "link"));
            nEp.Author                  = this.grabTagText(ep, "itunes:author");
            nEp.Description             = this.grabTagText(ep, "itunes:summary");

            // Get the Date Time conversion
            nEp.Date = (DateTime.TryParse(grabTagText(ep, "pubDate"), out timeTry)) ? timeTry : DateTime.MinValue;

            cLink = ep.SelectSingleNode("enclosure");
            if (cLink != null)
            {
                nEp.ContentInfo.Link = new Uri(cLink.Attributes["url"].Value);
                nEp.ContentInfo.Type = cLink.Attributes["type"].Value;
            }

            return nEp;
        }

        public List<Episode> parseEpisodes(XmlNodeList eps)
        {
            List<Episode> fEps = new List<Episode>();
            for (int i = 0; i < eps.Count; i++)
            {
                fEps.Add( this.parseEpisode(eps[i]) );
            }
            return fEps;
        }

        private string grabTagText(XmlNode root, string searchStr, string defaultStr = "")
        {
            XmlNode res = root.SelectSingleNode(searchStr, this.nsm);
            if (res != null)
            {
                return res.InnerText;
            }
            return defaultStr;
        }

    }
}
