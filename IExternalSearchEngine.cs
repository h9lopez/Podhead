using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastGrabber
{
    interface IExternalSearchEngine
    {
        Uri buildQueryString(SearchTerms terms);
        List<Series> searchSeries(SearchTerms terms);
    }
}
