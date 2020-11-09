﻿using Discord.Commands;
using Discord.WebSocket;
using Invite_Manager.Util;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Invite_Manager.Event.Commands
{
	[Group("channel")]
	public class SetChannel : ModuleBase<SocketCommandContext>
	{
        private readonly ChannelSettingsManager _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public SetChannel(IServiceProvider services)
        {
            _config = services.GetRequiredService<ChannelSettingsManager>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
        }


        [Command("set")]
		public async Task SetAsync()
        {
			ulong channelId = Context.Channel.Id;
            _config.SetInviteChannel(channelId);
			await ReplyAsync("Default channel succesfully set");
        }
	}
}
