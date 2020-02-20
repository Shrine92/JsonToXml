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
                MessageXmlModel xmlMess = new MessageXmlModel
                {
                    Message = mess.content,
                    Name = mess.sender_name,
                    Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(mess.timestamp_ms),
                    Type = mess.type
                };

                if (xmlMess.Message == null)
                {
                    if (mess.gifs != null)
                    {
                        xmlMess.Message = mess.gifs[0].uri;
                        xmlMess.Type = "Gif";
                    }
                    else if(mess.sticker != null)
                    {
                        List<string> tmpStr = mess.sticker.uri.Split('_').ToList();

                        tmpStr.RemoveAt(tmpStr.Count - 1);

                        xmlMess.Message = tmpStr.Aggregate((i,j) => i + '_' + j);
                        xmlMess.Type = "Sticker";
                    }
                    else if (mess.audio_files != null)
                    {
                        xmlMess.Message = mess.audio_files[0].uri;
                        xmlMess.Type = "AudioFile";
                    }
                    else if (mess.photos != null)
                    {
                        foreach (var photo in mess.photos)
                        {
                            xmlMessages.Add(new MessageXmlModel
                            {
                                Message = photo.uri,
                                Name = mess.sender_name,
                                Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(photo.creation_timestamp),
                                Type = "Photo"
                            });
                        }
                    }
                    else if (mess.files != null)
                    {
                        foreach (var file in mess.files)
                        {
                            xmlMessages.Add(new MessageXmlModel
                            {
                                Message = file.uri,
                                Name = mess.sender_name,
                                Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(file.creation_timestamp),
                                Type = "File"
                            });
                        }
                    }
                    else if (mess.videos != null)
                    {
                        foreach (var video in mess.videos)
                        {
                            xmlMessages.Add(new MessageXmlModel
                            {
                                Message = video.uri,
                                Name = mess.sender_name,
                                Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(video.creation_timestamp),
                                Type = "Video"
                            });
                        }
                    }

                }


                if (xmlMess.Message != null)
                {
                    xmlMessages.Add(xmlMess);
                }
                

                // add reaction to list
                if (mess.reactions != null)
                {
                    foreach (var reaction in mess.reactions)
                    {
                        xmlMessages.Add(new MessageXmlModel
                        {
                            Message = reaction.reaction,
                            Name = reaction.actor,
                            Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(mess.timestamp_ms),
                            Type = "Reaction"
                        });
                    }
                }
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
