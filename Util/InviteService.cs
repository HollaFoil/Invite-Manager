using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.Rest;
using System.IO;
using System.Xml.Serialization;

namespace Invite_Manager.Util
{
    public class Invite
    {
        public string id;
        public int? uses;
        public ulong inviterId;
        public Invite() { }
        public Invite(string _id, int? _uses, ulong _inviterId)
        {
            id = _id;
            uses = _uses;
            inviterId = _inviterId;
        }
    }
    public class InviteService
    {
        public void StoreInvites(IReadOnlyCollection<RestInviteMetadata> invites)
        {
            List<Invite> inviteList = new List<Invite>();
            foreach (RestInviteMetadata invite in invites)
            {
                Invite inv = new Invite(invite.Code, invite.Uses, invite.Inviter.Id);
                inviteList.Add(inv);
                WriteToXmlFile<List<Invite>>("invites.xml", inviteList);
            }
            
        }
        public List<Invite> GetStoredInvites()
        {
            return ReadFromXmlFile<List<Invite>>("invites.xml");

        }
        private static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        private static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
