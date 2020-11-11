using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.Rest;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

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
    public class User
    {
        public int invites;
        public ulong inviterId;
        public List<ulong> usersReffered;
        public User() {
            usersReffered = new List<ulong>();
        }
        public User(ulong _id)
        {
            inviterId = _id;
            invites = 0;
            usersReffered = new List<ulong>();
        }
    }
    public class InviteService
    {
        public User GetUser(ulong userId)
        {
            List<User> data = null;
            try
            {
                data = ReadFromXmlFile<List<User>>("InviteUses.xml");
            }
            catch (FileNotFoundException e)
            {
                data = new List<User>();
            }
            foreach (User user in data)
            {
                if (user.inviterId == userId) return user;
            }
            return null;
        }
        public void addBonusUserInvites(ulong userId, int amount)
        {
            List<User> data = null;
            try
            {
                data = ReadFromXmlFile<List<User>>("InviteUses.xml");
            }
            catch (FileNotFoundException e)
            {
                data = new List<User>();
            }
            bool contains = false;
            foreach (User user in data)
            {
                if (user.inviterId == userId)
                {
                    contains = true;
                    user.invites += amount;
                    break;
                }
            }
            if (!contains)
            {
                User user = new User(userId);
                user.invites = amount;
                data.Add(user);
            }
            storeUserData(data);
        }
        public void addUserInvite(ulong inviterId, ulong joinerId)
        {
            List<User> data = null;
            try
            {
                data = ReadFromXmlFile<List<User>>("InviteUses.xml");
            } catch (FileNotFoundException e)
            {
                data = new List<User>();
            }
            if (data == null) data = new List<User>();
            bool contains = false;
            foreach (User user in data)
            {
                if (user.inviterId == inviterId)
                {
                    contains = true;
                    user.invites++;
                    user.usersReffered.Add(joinerId);
                    break;
                }
            }
            if (!contains)
            {
                User user = new User(inviterId);
                user.invites = 1;
                user.usersReffered.Add(joinerId);
                data.Add(user);
            }
            storeUserData(data);
        }
        public void storeUserData(List<User> data)
        {
            WriteToXmlFile<List<User>>("InviteUses.xml", data);
        }
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
