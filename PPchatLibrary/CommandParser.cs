using System;
using System.Collections.Generic;

namespace PPchatLibrary
{
	using InvokersParametersPair = ValueTuple<IEnumerable<IInvoker<IApplication, object[]>>, object[]>;

	class CommandParser<Application> : IParser<IApplication, string, object[]>
		where Application : IApplication
	{
		static readonly ICommandsInfo commandsInfo = new CommandsSniffer<Application>();
		static readonly object[] arrayHelper = new object[1];

		static InvokersParametersPair JustOne(IEnumerable<IInvoker<IApplication, object[]>> commands, object o)
		{
			arrayHelper[0] = o;
			return (commands, arrayHelper);
		}

		static InvokersParametersPair JustOne(ICommandDescriptor command, object o)
			=> JustOne(command.AsSingleEnumerable(), o);
		
		static InvokersParametersPair ParseImplementation(string s)
		{
			var (head, tail) = Split(s);

			var commands = commandsInfo.GetValue(head);

			if (commands != null)
			{
				{
					var command = commands.GetIfOneLongArgument;
					if (command != null)
						return JustOne(command, tail);
				}

				var arguments = tail.Split(' ', 5, StringSplitOptions.RemoveEmptyEntries);
				var commandsWithRightArgumentCount = commands.GetValue(arguments.Length);
				if (commandsWithRightArgumentCount != null)
					return (commandsWithRightArgumentCount, arguments);
				else
					return JustOne(commandsInfo.BadArgumentCountCommand, arguments.Length);
			}
			else
				return JustOne(commandsInfo.NotFoundCommand, s.TrimStart());
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
