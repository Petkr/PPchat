using System;
using System.Collections.Generic;
using System.Reflection;

namespace PPchatLibrary
{


	class CommandsSniffer<Application> : BasicSniffer<string, ICommandArgumentCountDictionary, Application>, ICommandsInfo
		where Application : IApplication
	{
		public ICommandDescriptor NotFoundCommand { get; }
		public ICommandDescriptor BadArgumentCountCommand { get; }

		public CommandsSniffer()
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

			var commands = GetValue(attribute.Name);

			if (attribute.)
		}

		ICommandArgumentCountReadonlyDictionary? ISimpleReadonlyDictionary<string, ICommandArgumentCountReadonlyDictionary>.GetValue(string from)
			=> GetValue(from);
	}
}
