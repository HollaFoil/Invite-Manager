using Discord;
using Discord.Webhook;
using Discord.WebSocket;
using Invite_Manager.Util;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Invite_Manager.Event
{
    public class Events
    {
        private readonly ChannelSettingsManager _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public Events(IServiceProvider services)
        {
            _config = services.GetRequiredService<ChannelSettingsManager>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            
        }
        public async Task UserJoined(SocketGuildUser user)
        {
			ulong inviteChannelId = _config.GetInviteChannel();
			var channel = _discord.GetChannel(inviteChannelId) as IMessageChannel;
			await channel.SendMessageAsync("Welcome {0}" + user.Mention);
		}
	}
}
