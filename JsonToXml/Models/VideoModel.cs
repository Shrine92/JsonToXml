using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToXml.Models
{
    public class VideoModel
    {
        public string uri { get; set; }
        public int creation_timestamp { get; set; }
        public ThumbnailModel thumbnail { get; set; }
    }
}
