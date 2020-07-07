using PPchatLibrary;
using System.Net;

namespace PPchatClient
{
	[Command("connect", 1)]
	public readonly struct SimpleConnectAddressCommandArgument : ICommandArgument
	{
		public readonly IPAddress Address;

		public SimpleConnectAddressCommandArgument(string addressString)
		{
			Address = Parsers.ParseIPAddress(addressString);
		}
	}
}
