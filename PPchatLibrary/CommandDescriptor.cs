using System;

namespace PPchatLibrary
{
	class CommandDescriptor<Application> : BasicDescriptor<IApplication, ICommandArgument, Application>, ICommandDescriptor
		where Application : IApplication
	{
		readonly Type CommandArgumentType;
		public int ArgumentCount { get; }
		public int Priority { get; }

		public CommandDescriptor(Type commandArgumentType, int argumentCount, int priority = 0)
			: base(typeof(ICommandHandler<>), commandArgumentType)
		{
			CommandArgumentType = commandArgumentType;
			ArgumentCount = argumentCount;
			Priority = priority;
		}

		public void Invoke(IApplication application, object[]? parameters)
			=> Invoke(application, (ICommandArgument)Activator.CreateInstance(CommandArgumentType, parameters)!);
	}

	class CommandDescriptor<Application, CommandArgument> : CommandDescriptor<Application>
		where Application : IApplication
		where CommandArgument : ICommandArgument
	{
		public CommandDescriptor(int argumentCount = 0, int priority = 0)
			: base(typeof(CommandArgument), argumentCount, priority)
		{}
	}
}
