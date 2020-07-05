using System;

namespace PPchatLibrary
{
	public class CommandAttribute : Attribute
	{
		public string Name { get; }
		public bool OneLongArgument { get; }
		public int Priority { get; }

		public CommandAttribute(string name, int priority = 0)
		{
			Name = name;
			OneLongArgument = false;
			Priority = priority;
		}
		public CommandAttribute(string name, bool oneLongArgument)
		{
			Name = name;
			OneLongArgument = oneLongArgument;
			Priority = 0;
		}
	}
}
