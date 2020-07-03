namespace PPchatLibrary
{
	public readonly struct NotFoundCommandArgument : ICommandArgument
	{
		public readonly string Input;

		public NotFoundCommandArgument(string input)
		{
			Input = input;
		}
	}
}
