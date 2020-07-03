namespace PPchatLibrary
{
	public struct BadArgumentCountCommandArgument : ICommandArgument
	{
		public int count;

		public BadArgumentCountCommandArgument(int count)
		{
			this.count = count;
		}
	}
}
