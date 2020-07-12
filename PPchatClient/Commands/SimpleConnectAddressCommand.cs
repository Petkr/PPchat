using PPnetwork;
using System.Net;
using System;

namespace PPchatClient
{
	[Command("connect", 1)]
	public readonly struct SimpleConnectAddressCommandArgument : ICommandArgument
	{
		public readonly IPAddress Address;

		public SimpleConnectAddressCommandArgument(ReadOnlyMemory<char> addressString)
		{
			Address = Parsers.ParseIPAddress(addressString);
		}
	}
}
