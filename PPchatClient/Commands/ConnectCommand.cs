using System.Net;
using PPchatLibrary;

namespace PPchatClient
{
	[Command("connect")]
	public readonly struct ConnectCommandArgument : ICommandArgument
	{
		public readonly IPAddress ipAddress;
		public readonly int port;

		public ConnectCommandArgument(string ipAddressString, string portString)
		{
			ipAddress = Parsers.ParseIPAddress(ipAddressString);
			port = Parsers.ParsePort(portString);
		}
	}
}
