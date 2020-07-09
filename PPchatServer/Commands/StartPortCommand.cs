using PPchatLibrary;
using System;

namespace PPchatServer
{
	[Command("start")]
	public readonly struct StartPortCommandArgument : ICommandArgument
	{
		public readonly int Port;

		public StartPortCommandArgument(ReadOnlyMemory<char> portString)
		{
			Port = Parsers.ParsePort(portString);
		}
	}
}
