using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastGrabber
{
    interface IExternalRepoParser
    {
        List<Series> ParseJSON(string json);
    }
}
