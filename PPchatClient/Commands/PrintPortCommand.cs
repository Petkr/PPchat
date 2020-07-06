using PPchatLibrary;

namespace PPchatClient.Commands
{
	[Command("port", CommandFlags.UniqueArgumentCount)]
	public readonly struct PrintPortCommandArgument : ICommandArgument
	{ }
}
