using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Invite_Manager.Event.Commands
{
	public class SampleModule : ModuleBase<SocketCommandContext>
	{
		[Command("square")]
		[Summary("Squares a number.")]
		public async Task SquareAsync(
			[Summary("The number to square.")]
		int num)
		{
			// We can also access the channel from the Command Context.
			await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
		}

		[Command("userinfo")]
		[Summary
		("Returns info about the current user, or the user parameter, if one passed.")]
		[Alias("user", "whois")]
		public async Task UserInfoAsync(
			[Summary("The (optional) user to get info from")]
		SocketUser user = null)
		{
			var userInfo = user ?? Context.Client.CurrentUser;
			await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
		}
	}
}
