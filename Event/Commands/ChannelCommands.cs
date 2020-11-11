using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Invite_Manager.Util;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Invite_Manager.Event.Commands
{
	[Group("channel")]
	public class ChannelCommands : ModuleBase<SocketCommandContext>
	{
        private readonly ConfigService _config;

        public ChannelCommands(IServiceProvider services)
        {
            _config = services.GetRequiredService<ConfigService>();
        }


        [Command("set")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task SetAsync()
        {
			ulong channelId = Context.Channel.Id;
            _config.SetInviteChannel(channelId);
            ulong guildId = Context.Guild.Id;
            _config.SetDefaultGuild(guildId);
			await ReplyAsync("Default channel succesfully set");
        }
	}
}
