using PPnetwork;

namespace PPchatClient
{
	[Command("saved_servers", CommandFlags.UniqueName)]
	public readonly struct ListSavedServersCommandArgument : ICommandArgument
	{}
}
