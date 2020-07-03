using System;
using System.Collections.Generic;
using System.Linq;

namespace PPchatLibrary
{
	using InvokersParametersPair = ValueTuple<IEnumerable<IInvoker<IApplication, object[]>>, object[]>;

	class CommandParser<Application> : IParser<IApplication, string, object[]>
		where Application : IApplication
	{
		static readonly ICommandsInfo commandsInfo = new CommandsInfo<Application>();
		static readonly object[] arrayHelper = new object[1];

		static InvokersParametersPair ReturnJustOne(ICommandDescriptor command, object o)
		{
			arrayHelper[0] = o;
			return (command.AsSingleEnumerable(), arrayHelper);
		}
		
		static InvokersParametersPair ParseImplementation(string s)
		{
			var (head, tail) = Split(s);

			var commands = commandsInfo.GetInfo(head);

			if (commands != null)
			{
				if (commands.Any())
				{
					var first = commands.First();
					if (first.ArgumentCount == -1)
						return ReturnJustOne(first, tail);
				}
				else
					throw new Exception("Empty command enumerable for a command name.");

				var arguments = tail.Split(' ', 5, StringSplitOptions.RemoveEmptyEntries);
				var commandsWithRightArgumentCount = commands.Where(x => x.ArgumentCount == arguments.Length);
				if (commandsWithRightArgumentCount.Any())
					return (commandsWithRightArgumentCount, arguments);
				else
					return ReturnJustOne(commandsInfo.BadArgumentCountCommand, arguments.Length);
			}
			else
				return ReturnJustOne(commandsInfo.NotFoundCommand, s.TrimStart());
		}

		public (IInvoker<IApplication, object[]>, object[]) Parse(string input)
		{
			var (commands, arguments) = ParseImplementation(input);
			return (new EnumerableInvoker<IApplication, object[]>(commands), arguments);
		}

		static (string head, string tail) Split(string s)
		{
			var index = s.IndexOf(' ');
			if (index != -1)
				return (s.Substring(0, index), s.Substring(index));
			else
				return (s, "");
		}
	}
}
