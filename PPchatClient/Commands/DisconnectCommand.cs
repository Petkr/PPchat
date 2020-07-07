using PPchatLibrary;

namespace PPchatClient
{
	[Command("disconnect", CommandFlags.UniqueName)]
	public readonly struct DisconnectCommandArgument : ICommandArgument
	{ }
}
