using System;
using System.Collections.Generic;

namespace PPchatLibrary
{
	using InvokersParametersPair = ValueTuple<IEnumerable<IInvoker<IApplication, object[]>>, object[]>;

	class CommandParser<Application> : IParser<IApplication, string, object[]>
		where Application : IApplication
	{
		static readonly ICommandsSniffer commandsSniffer = new CommandsSniffer<Application>();
		static readonly object[] arrayHelper = new object[1];

		static InvokersParametersPair JustOneArgument(IEnumerable<IInvoker<IApplication, object[]>> commands, object o)
		{
			arrayHelper[0] = o;
			return (commands, arrayHelper);
		}

		static InvokersParametersPair JustOneArgument(ICommandDescriptor command, object o)
			=> JustOneArgument(command.AsSingleEnumerable(), o);
		
		static InvokersParametersPair ParseImplementation(string s)
		{
			var tokens = NativeParsing.GetTokensRange(s);

			var commands = commandsSniffer.GetValue(NativeParsing.GetMemory(s, tokens[0]));

			if (commands != null)
			{
				{
					var command = commands.GetIfOneLongArgument;
					if (command != null)
						return JustOneArgument(command, NativeParsing.GetTailMemory(s, tokens[0]).TrimStart());
				}
				tokens = tokens.Slice(1);

				var commandsWithRightArgumentCount = commands.GetValue(tokens.Length);
				if (commandsWithRightArgumentCount != null)
					return (commandsWithRightArgumentCount, NativeParsing.MakeMemories(s, tokens));
				else
					return JustOneArgument(commandsSniffer.BadArgumentCountCommand, tokens.Length);
			}
			else
				return JustOneArgument(commandsSniffer.NotFoundCommand, s.AsMemory().TrimStart());
		}

		public (IInvoker<IApplication, object[]>, object[]) Parse(string input)
		{
			var (commands, arguments) = ParseImplementation(input);
			NativeParsing.ReleaseResources();
			return (new EnumerableInvoker<IApplication, object[]>(commands), arguments);
		}
	}
}
