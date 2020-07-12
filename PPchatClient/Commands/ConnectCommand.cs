using System.Net;
using System;
using PPnetwork;

namespace PPchatClient
{
	[Command("connect", CommandFlags.UniqueArgumentCount)]
	public readonly struct ConnectCommandArgument : ICommandArgument
	{
		public readonly IPAddress ipAddress;
		public readonly int port;

		public ConnectCommandArgument(ReadOnlyMemory<char> ipAddressString, ReadOnlyMemory<char> portString)
		{
			ipAddress = Parsers.ParseIPAddress(ipAddressString);
			port = Parsers.ParsePort(portString);
		}
	}
}
