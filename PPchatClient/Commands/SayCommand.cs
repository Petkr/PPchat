using PPnetwork;
using System;

namespace PPchatClient
{
	[Command("say", CommandFlags.OneLongArgument | CommandFlags.UniqueName)]
	public readonly struct SayCommandArgument : ICommandArgument
	{
		public readonly ReadOnlyMemory<char> Message;

		public SayCommandArgument(ReadOnlyMemory<char> input)
		{
			Message = input;
		}
	}
}
