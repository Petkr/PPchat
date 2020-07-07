namespace PPchatLibrary
{
	public readonly struct BadArgumentCountCommandArgument : ICommandArgument
	{
		public readonly int Count;

		public BadArgumentCountCommandArgument(int count)
		{
			Count = count;
		}
	}
}
