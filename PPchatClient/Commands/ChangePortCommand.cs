using PPchatLibrary;

namespace PPchatClient.Commands
{
	[Command("port", CommandFlags.UniqueArgumentCount)]
	public readonly struct ChangePortCommandArgument : ICommandArgument
	{
		public readonly int port;

		public ChangePortCommandArgument(string s)
		{
			port = Parsers.ParsePort(s);
		}
	}
}
