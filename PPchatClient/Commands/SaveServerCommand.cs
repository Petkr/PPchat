using System;
using System.Net;
using PPnetwork;

namespace PPchatClient
{
	[Command("save", CommandFlags.UniqueName)]
	public readonly struct SaveServerCommandArgument : ICommandArgument
	{
		public readonly IPAddress Address;
		public readonly int Port;
		public readonly ReadOnlyMemory<char> ServerName;

		public SaveServerCommandArgument(ReadOnlyMemory<char> addressString, ReadOnlyMemory<char> portString, ReadOnlyMemory<char> serverName)
		{
			Address = Parsers.ParseIPAddress(addressString);
			Port = Parsers.ParsePort(portString);
			ServerName = serverName;
		}
	}
}
