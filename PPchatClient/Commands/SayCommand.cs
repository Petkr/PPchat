using PPchatLibrary;

namespace PPchatClient
{
	[Command("say", true)]
	public readonly struct SayCommandArgument : ICommandArgument
	{
		public readonly string Message;

		public SayCommandArgument(string input)
		{
			Message = input;
		}
	}
}
