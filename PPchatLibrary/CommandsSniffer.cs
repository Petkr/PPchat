using System;
using System.Collections.Generic;
using System.Reflection;

namespace PPchatLibrary
{
	class CommandsSniffer<Application> : BasicSniffer<ReadOnlyMemory<char>, ICommandArgumentCountDictionary, Application>, ICommandsSniffer
		where Application : IApplication
	{
		public ICommandDescriptor NotFoundCommand { get; }
		public ICommandDescriptor BadArgumentCountCommand { get; }

		public CommandsSniffer()
			: base(new SortedDictionary<ReadOnlyMemory<char>, ICommandArgumentCountDictionary>(new MemoryStringComparer()), typeof(ICommandHandler<>))
		{
			NotFoundCommand = new CommandDescriptor<Application, NotFoundCommandArgument>();
			BadArgumentCountCommand = new CommandDescriptor<Application, BadArgumentCountCommandArgument>();
		}

		protected override void Handle(Type commandArgumentType)
		{
			if (commandArgumentType == typeof(NotFoundCommandArgument) ||
				commandArgumentType == typeof(BadArgumentCountCommandArgument))
				return;

			var attribute = commandArgumentType.GetCustomAttribute<CommandAttribute>()!;
			var argumentCount = attribute.HasOneLongArgument ? 0 : commandArgumentType.GetFields().Length;
			var command = new CommandDescriptor<Application>(commandArgumentType, argumentCount, attribute.Priority);

			if (attribute.HasUniqueName)
			{
				ICommandArgumentCountDictionary commands;
				if (attribute.HasOneLongArgument)
					commands = new UniqueNameAndOneLongArgumentCommandArgumentCountDictionary(command);
				else
					commands = new UniqueNameCommandArgumentCountDictionary(command);
				Add(attribute.Name.AsMemory(), commands);
			}
			else
			{
				var commands = GetValue(attribute.Name.AsMemory());
				if (commands == null)
				{
					if (attribute.HasOneLongArgument)
						commands = new OneLongArgumentCommandArgumentCountDictionary();
					else
						commands = new BasicCommandArgumentCountDictionary();
					Add(attribute.Name.AsMemory(), commands);
				}

				if (attribute.HasOneLongArgument)
					commands.AddIfOneLongArgument(command);
				else
				{
					if (attribute.HasUniqueArgumentCount)
						commands.Add(argumentCount, new FakeCollection<ICommandDescriptor>(command.AsSingleEnumerable()));
					else
					{
						var commandsWithSameArgumentCount = commands.GetValue(argumentCount);
						if (commandsWithSameArgumentCount == null)
						{
							commandsWithSameArgumentCount = new SimpleCollection<SortedSet<ICommandDescriptor>, ICommandDescriptor> { command };
							commands.Add(argumentCount, commandsWithSameArgumentCount);
						}
						else
							commandsWithSameArgumentCount.Add(command);
					}
				}
			}
		}

		ICommandArgumentCountReadonlyDictionary? ISimpleReadonlyDictionary<ReadOnlyMemory<char>, ICommandArgumentCountReadonlyDictionary>.GetValue(ReadOnlyMemory<char> from)
			=> GetValue(from);
	}
}
