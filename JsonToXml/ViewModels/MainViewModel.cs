﻿using JsonToXml.Models;
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
        public List<string> JsonPaths { get; set; }

        public ConversationModel Conversation { get; set; }

        public List<string> Logs { get; set; }

        private List<MessageXmlModel> xmlMessages;

        private List<MessageXmlModel> xmlWords;

        private List<MessageXmlModel> xmlEmotes;

        private string regex = "(?:0\x20E3|1\x20E3|2\x20E3|3\x20E3|4\x20E3|5\x20E3|6\x20E3|7\x20E3|8\x20E3|9\x20E3|#\x20E3|\\*\x20E3|\xD83C(?:\xDDE6\xD83C(?:\xDDE8|\xDDE9|\xDDEA|\xDDEB|\xDDEC|\xDDEE|\xDDF1|\xDDF2|\xDDF4|\xDDF6|\xDDF7|\xDDF8|\xDDF9|\xDDFA|\xDDFC|\xDDFD|\xDDFF)|\xDDE7\xD83C(?:\xDDE6|\xDDE7|\xDDE9|\xDDEA|\xDDEB|\xDDEC|\xDDED|\xDDEE|\xDDEF|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF6|\xDDF7|\xDDF8|\xDDF9|\xDDFB|\xDDFC|\xDDFE|\xDDFF)|\xDDE8\xD83C(?:\xDDE6|\xDDE8|\xDDE9|\xDDEB|\xDDEC|\xDDED|\xDDEE|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF5|\xDDF7|\xDDFA|\xDDFB|\xDDFC|\xDDFD|\xDDFE|\xDDFF)|\xDDE9\xD83C(?:\xDDEA|\xDDEC|\xDDEF|\xDDF0|\xDDF2|\xDDF4|\xDDFF)|\xDDEA\xD83C(?:\xDDE6|\xDDE8|\xDDEA|\xDDEC|\xDDED|\xDDF7|\xDDF8|\xDDF9|\xDDFA)|\xDDEB\xD83C(?:\xDDEE|\xDDEF|\xDDF0|\xDDF2|\xDDF4|\xDDF7)|\xDDEC\xD83C(?:\xDDE6|\xDDE7|\xDDE9|\xDDEA|\xDDEB|\xDDEC|\xDDED|\xDDEE|\xDDF1|\xDDF2|\xDDF3|\xDDF5|\xDDF6|\xDDF7|\xDDF8|\xDDF9|\xDDFA|\xDDFC|\xDDFE)|\xDDED\xD83C(?:\xDDF0|\xDDF2|\xDDF3|\xDDF7|\xDDF9|\xDDFA)|\xDDEE\xD83C(?:\xDDE8|\xDDE9|\xDDEA|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF6|\xDDF7|\xDDF8|\xDDF9)|\xDDEF\xD83C(?:\xDDEA|\xDDF2|\xDDF4|\xDDF5)|\xDDF0\xD83C(?:\xDDEA|\xDDEC|\xDDED|\xDDEE|\xDDF2|\xDDF3|\xDDF5|\xDDF7|\xDDFC|\xDDFE|\xDDFF)|\xDDF1\xD83C(?:\xDDE6|\xDDE7|\xDDE8|\xDDEE|\xDDF0|\xDDF7|\xDDF8|\xDDF9|\xDDFA|\xDDFB|\xDDFE)|\xDDF2\xD83C(?:\xDDE6|\xDDE8|\xDDE9|\xDDEA|\xDDEB|\xDDEC|\xDDED|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF5|\xDDF6|\xDDF7|\xDDF8|\xDDF9|\xDDFA|\xDDFB|\xDDFC|\xDDFD|\xDDFE|\xDDFF)|\xDDF3\xD83C(?:\xDDE6|\xDDE8|\xDDEA|\xDDEB|\xDDEC|\xDDEE|\xDDF1|\xDDF4|\xDDF5|\xDDF7|\xDDFA|\xDDFF)|\xDDF4\xD83C\xDDF2|\xDDF5\xD83C(?:\xDDE6|\xDDEA|\xDDEB|\xDDEC|\xDDED|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF7|\xDDF8|\xDDF9|\xDDFC|\xDDFE)|\xDDF6\xD83C\xDDE6|\xDDF7\xD83C(?:\xDDEA|\xDDF4|\xDDF8|\xDDFA|\xDDFC)|\xDDF8\xD83C(?:\xDDE6|\xDDE7|\xDDE8|\xDDE9|\xDDEA|\xDDEC|\xDDED|\xDDEE|\xDDEF|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF7|\xDDF8|\xDDF9|\xDDFB|\xDDFD|\xDDFE|\xDDFF)|\xDDF9\xD83C(?:\xDDE6|\xDDE8|\xDDE9|\xDDEB|\xDDEC|\xDDED|\xDDEF|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF7|\xDDF9|\xDDFB|\xDDFC|\xDDFF)|\xDDFA\xD83C(?:\xDDE6|\xDDEC|\xDDF2|\xDDF8|\xDDFE|\xDDFF)|\xDDFB\xD83C(?:\xDDE6|\xDDE8|\xDDEA|\xDDEC|\xDDEE|\xDDF3|\xDDFA)|\xDDFC\xD83C(?:\xDDEB|\xDDF8)|\xDDFD\xD83C\xDDF0|\xDDFE\xD83C(?:\xDDEA|\xDDF9)|\xDDFF\xD83C(?:\xDDE6|\xDDF2|\xDDFC)))|[\xA9\xAE\x203C\x2049\x2122\x2139\x2194-\x2199\x21A9\x21AA\x231A\x231B\x2328\x23CF\x23E9-\x23F3\x23F8-\x23FA\x24C2\x25AA\x25AB\x25B6\x25C0\x25FB-\x25FE\x2600-\x2604\x260E\x2611\x2614\x2615\x2618\x261D\x2620\x2622\x2623\x2626\x262A\x262E\x262F\x2638-\x263A\x2648-\x2653\x2660\x2663\x2665\x2666\x2668\x267B\x267F\x2692-\x2694\x2696\x2697\x2699\x269B\x269C\x26A0\x26A1\x26AA\x26AB\x26B0\x26B1\x26BD\x26BE\x26C4\x26C5\x26C8\x26CE\x26CF\x26D1\x26D3\x26D4\x26E9\x26EA\x26F0-\x26F5\x26F7-\x26FA\x26FD\x2702\x2705\x2708-\x270D\x270F\x2712\x2714\x2716\x271D\x2721\x2728\x2733\x2734\x2744\x2747\x274C\x274E\x2753-\x2755\x2757\x2763\x2764\x2795-\x2797\x27A1\x27B0\x27BF\x2934\x2935\x2B05-\x2B07\x2B1B\x2B1C\x2B50\x2B55\x3030\x303D\x3297\x3299]|\xD83C[\xDC04\xDCCF\xDD70\xDD71\xDD7E\xDD7F\xDD8E\xDD91-\xDD9A\xDE01\xDE02\xDE1A\xDE2F\xDE32-\xDE3A\xDE50\xDE51\xDF00-\xDF21\xDF24-\xDF93\xDF96\xDF97\xDF99-\xDF9B\xDF9E-\xDFF0\xDFF3-\xDFF5\xDFF7-\xDFFF]|\xD83D[\xDC00-\xDCFD\xDCFF-\xDD3D\xDD49-\xDD4E\xDD50-\xDD67\xDD6F\xDD70\xDD73-\xDD79\xDD87\xDD8A-\xDD8D\xDD90\xDD95\xDD96\xDDA5\xDDA8\xDDB1\xDDB2\xDDBC\xDDC2-\xDDC4\xDDD1-\xDDD3\xDDDC-\xDDDE\xDDE1\xDDE3\xDDEF\xDDF3\xDDFA-\xDE4F\xDE80-\xDEC5\xDECB-\xDED0\xDEE0-\xDEE5\xDEE9\xDEEB\xDEEC\xDEF0\xDEF3]|\xD83E[\xDD10-\xDD18\xDD80-\xDD84\xDDC0]";

        private string regexSC = "[!@#$%§^&*(),.?\"/\\:{}|<>]";

        public bool ReadFiles()
        {
            Logs.Add("Read file operation begins.");

            Conversation = new ConversationModel();
            Conversation.messages = new List<MessageModel>();

            foreach (string path in JsonPaths)
            {
                try
                {
                    using (StreamReader r = new StreamReader(path))
                    {
                        Logs.Add("File " + path.Split('\\').Last() + "has begin to be read");
                        string json = r.ReadToEnd();

                        string parsedString = Regex.Unescape(json.Replace("\\\"", "").Replace("\\\\", ""));

                        byte[] isoBites = Encoding.GetEncoding("ISO-8859-1").GetBytes(parsedString);

                        json = Encoding.UTF8.GetString(isoBites, 0, isoBites.Length);

                        ConversationModel tmpConvers = JsonConvert.DeserializeObject<ConversationModel>(json);

                        Conversation.participants = tmpConvers.participants;
                        Conversation.title = tmpConvers.title;
                        Conversation.messages.AddRange(tmpConvers.messages);

                        Logs.Add("File " + path.Split('/').Last() + "has finished to be read");
                    }
                }
                catch (Exception ex)
                {
                    Logs.Add("Error:" + ex.Message);

                    return false;
                }

            }

            return true;
        }

        public void ConvertToXmlFiles()
        {
            xmlMessages = new List<MessageXmlModel>();
            xmlWords = new List<MessageXmlModel>();
            xmlEmotes = new List<MessageXmlModel>();

            foreach (MessageModel mess in Conversation.messages)
            {
                MessageXmlModel xmlMess = new MessageXmlModel
                {
                    Message = mess.content,
                    Name = mess.sender_name,
                    Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(mess.timestamp_ms),
                    Type = mess.type
                };

                // Logs.Add("Processing started for message at the date: " + xmlMess.Date);

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

                // create emote file
                if (mess.content != null)
                {
                    this.CreateEmoteFile(mess);
                    this.CreateWordFile(mess);
                }

                // Logs.Add("Processing finished for message at the date: " + xmlMess.Date);

            }

            this.WriteToXml();

        }

        private void CreateEmoteFile(MessageModel mess)
        {
            var results = Regex.Matches(mess.content, regex);

            foreach (Match match in results)
            {
                xmlEmotes.Add(new MessageXmlModel
                {
                    Message = match.Value,
                    Name = mess.sender_name,
                    Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(mess.timestamp_ms),
                    Type = "Emote"
                });
            }
        }

        private void CreateWordFile(MessageModel mess)
        {
            var results = mess.content.Split(' ');

            foreach (var word in results)
            {
                if (!Regex.IsMatch(word, regexSC))
                {
                    xmlWords.Add(new MessageXmlModel
                    {
                        Message = word,
                        Name = mess.sender_name,
                        Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(mess.timestamp_ms),
                        Type = "Word"
                    });
                }
            }
        }

        private void WriteToXml()
        {
            Logs.Add("Writing to xml files begon");

            if (xmlMessages != null && xmlMessages.Count > 0)
            {
                try
                {
                    Logs.Add("Writing messages to xml started");
                    XmlSerializer writer = new XmlSerializer(typeof(List<MessageXmlModel>));

                    var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//MessageFile.xml";
                    FileStream file = File.Create(path);

                    writer.Serialize(file, xmlMessages);
                    file.Close();
                    Logs.Add("Writing messages to xml ended");
                }
                catch (Exception ex)
                {
                    Logs.Add("Error while writing to xml for messages: \n" + ex.Message);
                }
                
            }

            if (xmlEmotes != null && xmlEmotes.Count > 0)
            {
                try
                {
                    Logs.Add("Writing emotes to xml started");
                    XmlSerializer writer = new XmlSerializer(typeof(List<MessageXmlModel>));

                    var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//EmoteFile.xml";
                    FileStream file = File.Create(path);

                    writer.Serialize(file, xmlEmotes);
                    file.Close();
                    Logs.Add("Writing emotes to xml ended");
                }
                catch (Exception ex)
                {
                    Logs.Add("Error while writing to xml for emotes: \n" + ex.Message);
                }
                
            }

            if (xmlWords != null && xmlWords.Count > 0)
            {
                try
                {
                    Logs.Add("Writing words to xml started");
                    XmlSerializer writer = new XmlSerializer(typeof(List<MessageXmlModel>));

                    var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//WordFile.xml";
                    FileStream file = File.Create(path);

                    writer.Serialize(file, xmlWords);
                    file.Close();
                    Logs.Add("Writing words to xml ended");
                }
                catch (Exception ex)
                {
                    Logs.Add("Error while writing to xml for words: \n" + ex.Message);
                }
               
            }
        }

    }
}
