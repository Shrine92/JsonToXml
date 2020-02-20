using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToXml.Models
{
    public class MessageModel
    {
        public string sender_name { get; set; }
        public double timestamp_ms { get; set; }
        public string content { get; set; }
        public string type { get; set; }
        public List<GifModel> gifs { get; set; }
        public StickerModel sticker { get; set; }
        public List<VideoModel> videos { get; set; }
        public List<ReactionModel> reactions { get; set; }
        public ShareModel share { get; set; }
        public int? call_duration { get; set; }
        public List<PhotoModel> photos { get; set; }
        public List<AudioFileModel> audio_files { get; set; }
        public List<FileModel> files { get; set; }
        public bool? missed { get; set; }
    }
}
