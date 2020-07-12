using PPnetwork;

namespace PPchatClient
{
	[Command("port", CommandFlags.UniqueArgumentCount)]
	public readonly struct PrintPortCommandArgument : ICommandArgument
	{ }
}
