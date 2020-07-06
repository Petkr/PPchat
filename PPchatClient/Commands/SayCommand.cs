using PPchatLibrary;

namespace PPchatClient
{
	[Command("say", CommandFlags.OneLongArgument | CommandFlags.UniqueName)]
	public readonly struct SayCommandArgument : ICommandArgument
	{
		public readonly string Message;

		public SayCommandArgument(string input)
		{
			Message = input;
		}
	}
}
