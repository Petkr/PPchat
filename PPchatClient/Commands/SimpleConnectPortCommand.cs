using PPnetwork;
using System;

namespace PPchatClient
{
	[Command("connect", 0)]
	public readonly struct SimpleConnectPortCommandArgument : ICommandArgument
	{
		public readonly int Port;

		public SimpleConnectPortCommandArgument(ReadOnlyMemory<char> portString)
		{
			Port = Parsers.ParsePort(portString);
		}
	}
}
