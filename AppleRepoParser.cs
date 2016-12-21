using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PodcastGrabber
{
    class AppleRepoDeserializeFrame
    {
        public string trackName;
        public string artistName;
        public string collectionName;
        public string artworkUrl100;
        public string artworkUrl160;
        public string feedUrl;
        public List<string> genres;
        public List<string> genreIds;

        public override string ToString()
        {
            return String.Format("trackName:\t{0}\nartistName:\t{1}\ncollectionName:\t{2}\nartworkUrl100:\t{3}\nartworkUrl160:{4}\nfeedUrl:\t{5}",
                                    trackName, artistName, collectionName, artworkUrl100, artworkUrl160, feedUrl);
        }
    }

    class AppleRepoParser : IExternalRepoParser
    {
        public List<Series> ParseJSON(string json)
        {
            JObject obj = JObject.Parse(json);
            int resultCount = (int)obj["resultCount"];
            List<JToken> results = obj["results"].Children().ToList();
            List<Series> parsedResults = new List<Series>();

            foreach (JToken res in results)
            {
                parsedResults.Add( parseSingleResult(res) );
            }

            return parsedResults;
        }

        private Series parseSingleResult(JToken res)
        {
            // Do sanity check that the result is a podcast type.
            string kind = (string)res["kind"];
            string type = (string)res["wrapperType"];
            Series newSeries = new Series();

            if (!kind.Contains("podcast"))
            {
                Console.WriteLine( String.Format("Wrong type of search result, kind: {0}, type: {1}", kind, type) );
                return null;
            }

            AppleRepoDeserializeFrame frame = JsonConvert.DeserializeObject<AppleRepoDeserializeFrame>(res.ToString());
            newSeries.Name = frame.trackName;
            newSeries.Author = frame.artistName;
            newSeries.FeedLink = frame.feedUrl;

            return newSeries;
        }
    }
}
