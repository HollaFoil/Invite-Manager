using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Invite_Manager.Util;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Invite_Manager.Event.Commands
{
    [Group("stats")]
    public class StatsCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ConfigService _configervice;
        private readonly InviteService _inviteService;

        public StatsCommands(IServiceProvider services)
        {
            _configervice = services.GetRequiredService<ConfigService>();
            _inviteService = services.GetRequiredService<InviteService>();
        }


        [Command("me")]
        public async Task SetAsync()
        {
            User user = _inviteService.GetUser(Context.User.Id);
            if (user == null) await ReplyAsync("You have no invites.");
            else await ReplyAsync("You have " + user.invites + " invites.");
        }
    }
}