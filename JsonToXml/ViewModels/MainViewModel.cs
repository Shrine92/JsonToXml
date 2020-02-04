using JsonToXml.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToXml.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public List<string> JsonPaths { get; set; }

        public ConversationModel conversation;

        public bool ReadFiles()
        {
            conversation = new ConversationModel();

            foreach (string path in JsonPaths)
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    ConversationModel tmpConvers = JsonConvert.DeserializeObject<ConversationModel>(json);

                    conversation.participants = tmpConvers.participants;
                    conversation.title = tmpConvers.title;
                    conversation.messages.AddRange(tmpConvers.messages);
                }
            }

            return true;
        }
    }
}
