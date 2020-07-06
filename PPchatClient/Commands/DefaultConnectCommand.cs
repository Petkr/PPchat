using PPchatLibrary;

namespace PPchatClient
{
	[Command("connect", CommandFlags.UniqueArgumentCount)]
	public readonly struct DefaultConnectCommandArgument : ICommandArgument
	{ }
}
