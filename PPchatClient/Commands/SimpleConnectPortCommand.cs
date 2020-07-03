using PPchatLibrary;

namespace PPchatClient
{
	[Command("connect", 0)]
	public readonly struct SimpleConnectPortCommandArgument : ICommandArgument
	{
		public readonly int port;

		public SimpleConnectPortCommandArgument(string portString)
		{
			port = Parsers.ParsePort(portString);
		}
	}
}
