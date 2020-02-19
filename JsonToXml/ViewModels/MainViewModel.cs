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
using System.Xml.Serialization;

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

        private List<MessageXmlModel> xmlMessages;

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

                        string parsedString = Regex.Unescape(json.Replace("\\\"", ""));

                        byte[] isoBites = Encoding.GetEncoding("ISO-8859-1").GetBytes(parsedString);

                        json = Encoding.UTF8.GetString(isoBites, 0, isoBites.Length);

                        ConversationModel tmpConvers = JsonConvert.DeserializeObject<ConversationModel>(json);

                        conversation.participants = tmpConvers.participants;
                        conversation.title = tmpConvers.title;
                        conversation.messages.AddRange(tmpConvers.messages);
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

        public void ConvertToXmlFiles()
        {
            xmlMessages = new List<MessageXmlModel>();

            foreach (MessageModel mess in conversation.messages)
            {
                xmlMessages.Add(new MessageXmlModel
                {
                    Message = mess.content,
                    Name = mess.sender_name,
                    Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(mess.timestamp_ms),
                    Type = mess.type
                });
            }

            this.WriteToXml();

        }

        private void WriteToXml()
        {
            if (xmlMessages != null && xmlMessages.Count > 0)
            {
                XmlSerializer writer = new XmlSerializer(typeof(List<MessageXmlModel>));

                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationOverview.xml";
                FileStream file = File.Create(path);

                writer.Serialize(file, xmlMessages);
                file.Close();
            }
        }

    }
}
