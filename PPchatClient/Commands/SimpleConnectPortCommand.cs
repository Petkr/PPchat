using PPchatLibrary;

namespace PPchatClient
{
	[Command("connect", 0)]
	public readonly struct SimpleConnectPortCommandArgument : ICommandArgument
	{
		public readonly int Port;

		public SimpleConnectPortCommandArgument(string portString)
		{
			Port = Parsers.ParsePort(portString);
		}
	}
}
