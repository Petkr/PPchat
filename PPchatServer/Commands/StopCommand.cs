using PPnetwork;

namespace PPchatServer
{
	[Command("stop", CommandFlags.UniqueName)]
	public readonly struct StopCommandArgument : ICommandArgument
	{ }
}
