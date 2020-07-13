using PPnetwork;

namespace PPchatServer
{
	[Command("start", CommandFlags.UniqueArgumentCount)]
	public readonly struct StartCommandArgument : ICommandArgument
	{ }
}
