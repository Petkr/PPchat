using PPchatLibrary;
using System;

namespace PPchatClient
{
	[Command("connect", 2)]
	public readonly struct SimpleConnectSavedServerCommandArgument : ICommandArgument
	{
		public readonly ReadOnlyMemory<char> ServerName;

		public SimpleConnectSavedServerCommandArgument(ReadOnlyMemory<char> serverName)
		{
			ServerName = serverName;
		}
	}
}
