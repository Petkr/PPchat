using PPnetwork;
using System;

namespace PPchatServer
{
	[Command("start", CommandFlags.UniqueArgumentCount)]
	public readonly struct StartPortCommandArgument : ICommandArgument
	{
		public readonly int Port;

		public StartPortCommandArgument(ReadOnlyMemory<char> portString)
		{
			Port = Parsers.ParsePort(portString);
		}
	}
}
