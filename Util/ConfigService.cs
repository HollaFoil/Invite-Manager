using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace Invite_Manager.Util
{
    class ConfigService
    {
        public ulong GetInviteChannel()
            => ulong.Parse(ConfigurationManager.AppSettings.Get("chnl"));
        public ulong GetDefaultGuild()
            => ulong.Parse(ConfigurationManager.AppSettings.Get("guild"));
        public void SetInviteChannel(ulong id)
        {
            SetConfigSetting("chnl", id.ToString());
        }
        public void SetDefaultGuild(ulong id)
        {
            SetConfigSetting("guild", id.ToString());
        }
        public void SetConfigSetting(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                    settings.Add(key, value);
                else
                    settings[key].Value = value;
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
