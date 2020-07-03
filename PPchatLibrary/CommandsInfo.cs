using System;
using System.Collections.Generic;
using System.Reflection;

namespace PPchatLibrary
{
	class CommandsInfo<Application> : BasicInfo<string, ICollection<ICommandDescriptor>, Application>, ICommandsInfo
		where Application : IApplication
	{
		public ICommandDescriptor NotFoundCommand { get; }
		public ICommandDescriptor BadArgumentCountCommand { get; }

		public CommandsInfo()
			: base(typeof(ICommandHandler<>))
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
			var argumentCount = attribute.OneLongArgument ? -1 : commandArgumentType.GetFields().Length;
			var command = new CommandDescriptor<Application>(commandArgumentType, argumentCount, attribute.Priority);

			var commands = GetInfo(attribute.Name);

			if (commands != null)
				commands.Add(command);
			else
				AddInfo(attribute.Name, new SortedSet<ICommandDescriptor>() { command });
		}

		IEnumerable<ICommandDescriptor>? IInfo<string, IEnumerable<ICommandDescriptor>>.GetInfo(string input) => GetInfo(input);
	}
}
