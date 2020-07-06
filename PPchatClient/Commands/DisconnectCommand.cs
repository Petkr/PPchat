using PPchatLibrary;

namespace PPchatClient.Commands
{
	[Command("disconnect", CommandFlags.UniqueName)]
	public readonly struct DisconnectCommandArgument : ICommandArgument
	{ }
}
