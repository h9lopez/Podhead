using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;

namespace PodcastGrabber
{
    class AppleSearchEngine : IExternalSearchEngine
    {
        private string AppleBaseString = "https://itunes.apple.com/search";

        public Uri buildQueryString(SearchTerms terms)
        {
            UriBuilder query = new UriBuilder(this.AppleBaseString);
            var parameters = HttpUtility.ParseQueryString(String.Empty);
            parameters["term"] = terms.rawString;
            parameters["media"] = "podcast";
            parameters["entity"] = "podcast";
            parameters["attribute"] = "titleTerm";
            parameters["limit"] = "100";
            query.Query = parameters.ToString();

            return query.Uri;
        }

        public List<Series> searchSeries(SearchTerms terms)
        {
            Uri searchUri = buildQueryString(terms);
            AppleRepoParser repoParser = new AppleRepoParser();
            List<Series> parsedResults = null;

            WebRequest req = WebRequest.Create(searchUri);
            req.Method = WebRequestMethods.Http.Get;
            req.ContentType = "application/json";
            var res = (HttpWebResponse)req.GetResponse();
            string ptext;

            using (var sr = new StreamReader(res.GetResponseStream()))
            {
                ptext = sr.ReadToEnd();
            }

            parsedResults = repoParser.ParseJSON(ptext);

            return parsedResults;
        }
    }
}
