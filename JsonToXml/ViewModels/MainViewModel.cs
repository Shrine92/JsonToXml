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
        private List<string> jsonPaths;

        public List<string> JsonPaths
        {
            get { return jsonPaths; }
            set { jsonPaths = value; }
        }

        private ConversationModel conversation;

        public ConversationModel Conversation
        {
            get { return conversation; }
            set { conversation = value; }
        }

        public bool ReadFiles()
        {
            conversation = new ConversationModel();
            conversation.messages = new List<MessageModel>();

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
