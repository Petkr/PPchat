using PPchatLibrary;
using System;

namespace PPchatClient
{
	[Command("port", CommandFlags.UniqueArgumentCount)]
	public readonly struct ChangePortCommandArgument : ICommandArgument
	{
		public readonly int port;

		public ChangePortCommandArgument(ReadOnlyMemory<char> s)
		{
			port = Parsers.ParsePort(s);
		}
	}
}
