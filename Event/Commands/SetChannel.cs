using Discord.Commands;
using System.Threading.Tasks;

namespace Invite_Manager.Event.Commands
{
	public class SetChannel : ModuleBase<SocketCommandContext>
	{
		[Command("ping")]
		[Alias("pong", "hello")]
		public Task PingAsync()
			=> ReplyAsync("pong!");
	}
}
