using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastGrabber
{
    class Series
    {
        public string Name;
        public string Author;
        public string FeedLink;
        List<Episode> Episodes;
    }

    class Episode
    {
        public string Name;
        public DateTime Date;
        public string Description;
        public Uri Link;
        public string Author;
        public ContentMetdata ContentInfo;

        public Episode()
        {
            this.ContentInfo = new ContentMetdata();
        }

        public override string ToString()
        {
            return (String.Format("Name:\t\t{0}\nDate:\t\t{1}\nDesc:\t\t{2}\nLink:\t\t{3}\nDuration:\t\t{4}\nURL:\t\t{5}\nType:\t\t{6}\n",
                    this.Name, this.Date, this.Description, this.Link, this.ContentInfo.Duration, this.ContentInfo.Link, this.ContentInfo.Type ));
        }
    }

    class ContentMetdata
    {
        public Uri Link;
        public string Type;
        public string Duration;
    }


}
