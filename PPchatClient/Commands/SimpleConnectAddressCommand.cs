using PPchatLibrary;
using System.Net;

namespace PPchatClient
{
	[Command("connect", 1)]
	public readonly struct SimpleConnectAddressCommandArgument : ICommandArgument
	{
		public readonly IPAddress address;

		public SimpleConnectAddressCommandArgument(string addressString)
		{
			address = Parsers.ParseIPAddress(addressString);
		}
	}
}
