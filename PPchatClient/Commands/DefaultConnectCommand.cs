using PPnetwork;

namespace PPchatClient
{
	[Command("connect", CommandFlags.UniqueArgumentCount)]
	public readonly struct DefaultConnectCommandArgument : ICommandArgument
	{ }
}
