using System;
using System.Net;
using PPchatLibrary;

namespace PPchatClient
{
	[Command("save", CommandFlags.UniqueName)]
	public readonly struct SaveServerCommandArgument : ICommandArgument
	{
		public readonly IPAddress Address;
		public readonly int Port;
		public readonly string ServerName;

		public SaveServerCommandArgument(string addressString, string portString, string serverName)
		{
			Address = Parsers.ParseIPAddress(addressString);
			Port = Parsers.ParsePort(portString);
			ServerName = serverName;
		}
	}
}
