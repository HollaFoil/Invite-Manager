using Discord;
using Discord.Rest;
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
        private readonly ConfigService _configService;
        private readonly DiscordSocketClient _discord;
        private readonly InviteService _inviteService;
        private SocketGuild guild;

        public Events(IServiceProvider services)
        {
            _configService = services.GetRequiredService<ConfigService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _inviteService = services.GetRequiredService<InviteService>();
            
        }
        public async Task AnnounceUserJoined(SocketGuildUser user)
        {
            List<Invite> _storedInvites = _inviteService.GetStoredInvites();
            Dictionary<string, Invite> storedInvites = new Dictionary<string, Invite>();
            foreach (Invite invite in _storedInvites)
                storedInvites.Add(invite.id, invite);
            IReadOnlyCollection<RestInviteMetadata> updatedInvites = guild.GetInvitesAsync().Result;

            bool found = false;
            RestUser inviter = null;
            foreach (RestInviteMetadata invite in updatedInvites)
            {
                if (storedInvites.ContainsKey(invite.Id))
                {
                    if (invite.Uses > storedInvites[invite.Id].uses)
                    {
                        inviter = invite.Inviter;
                        found = true;
                        break;
                    }
                }
                else if (invite.Uses > 0)
                {
                    inviter = invite.Inviter;
                    found = true;
                    break;
                }
            }
            _inviteService.StoreInvites(updatedInvites);

            ulong inviteChannelId = _configService.GetInviteChannel();
            var channel = _discord.GetChannel(inviteChannelId) as SocketTextChannel;
            if (inviter == null)
                await channel.SendMessageAsync(user.Mention + " has been invited by a dark force.");
            else
            {
                _inviteService.addUserInvite(inviter.Id, user.Id);
                await channel.SendMessageAsync(user.Mention + " has been invited by " + inviter.Mention);
            }
		}
        public async Task onReady()
        {
            guild = _discord.GetGuild(_configService.GetDefaultGuild());
            Task<IReadOnlyCollection<RestInviteMetadata>> invites = guild.GetInvitesAsync();
            _inviteService.StoreInvites(invites.Result);
        }
    }
}
