using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace Invite_Manager.Util
{
    class ChannelSettingsManager
    {
        public ulong GetInviteChannel()
            => ulong.Parse(ConfigurationManager.AppSettings.Get("InviteChannelId"));
        public void SetInviteChannel(ulong id)
        {
            ConfigurationManager.AppSettings.Set("InviteChannelId", id.ToString());
        }
    }
}
