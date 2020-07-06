using System;

namespace PPchatLibrary
{
	[Flags]
	public enum CommandFlags
	{
		None = 0b000,
		UniqueName = 0b001,
		UniqueArgumentCount = 0b010,
		OneLongArgument = 0b100,
	}

	public class CommandAttribute : Attribute
	{
		public string Name { get; }
		public int Priority { get; }
		readonly CommandFlags Flags;

		public CommandAttribute(string name)
			: this(name, 0)
		{}
		public CommandAttribute(string name, CommandFlags flags)
		  : this(name, 0, flags)
		{ }
		public CommandAttribute(string name, int priority)
		  : this(name, priority, CommandFlags.None)
		{ }
		CommandAttribute(string name, int priority, CommandFlags flags)
		{
			Name = name;
			Priority = priority;
			Flags = flags;
		}

		public bool HasOneLongArgument => Flags.HasFlag(CommandFlags.OneLongArgument);
		public bool HasUniqueName => Flags.HasFlag(CommandFlags.UniqueName);
		public bool HasUniqueArgumentCount => HasOneLongArgument || HasUniqueName || Flags.HasFlag(CommandFlags.UniqueArgumentCount);
	}
}
