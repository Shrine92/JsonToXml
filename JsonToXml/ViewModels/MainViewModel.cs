using JsonToXml.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                try
                {
                    using (StreamReader r = new StreamReader(path))
                    {
                        string json = r.ReadToEnd();

                        string parsedString = Regex.Unescape(json);

                        byte[] isoBites = Encoding.GetEncoding("ISO-8859-1").GetBytes(parsedString);

                        ConversationModel tmpConvers = JsonConvert.DeserializeObject<ConversationModel>(Encoding.UTF8.GetString(isoBites, 0, isoBites.Length));

                        conversation.participants = tmpConvers.participants;
                        conversation.title = tmpConvers.title;
                        conversation.messages.AddRange(tmpConvers.messages);

                        Debug.WriteLine(tmpConvers.title);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                    return false;
                }

            }

            return true;
        }

    }
}
