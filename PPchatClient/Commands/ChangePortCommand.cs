using PPchatLibrary;

namespace PPchatClient.Commands
{
	[Command("port")]
	public readonly struct ChangePortCommandArgument : ICommandArgument
	{
		public readonly int port;

		public ChangePortCommandArgument(string s)
		{
			port = Parsers.ParsePort(s);
		}
	}
}
