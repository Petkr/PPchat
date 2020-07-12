using PPnetwork;

namespace PPchatClient
{
	[Command("disconnect", CommandFlags.UniqueName)]
	public readonly struct DisconnectCommandArgument : ICommandArgument
	{ }
}
