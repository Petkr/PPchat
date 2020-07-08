using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace PPchatLibrary
{
	using InvokersParametersPair = ValueTuple<IEnumerable<IInvoker<IApplication, object[]>>, object[]>;

	public static class NativeParsing
	{
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public unsafe readonly struct MySpan<T>
			where T : unmanaged
		{
			public readonly T* Begin;
			public readonly T* End;

			public MySpan(T* begin, T* end)
			{
				Begin = begin;
				End = end;
			}

			public int Length => (int)(End - Begin);

			public Span<T> AsSpan() => new Span<T>(Begin, Length);

			public static implicit operator Span<T>(MySpan<T> s) => s.AsSpan();
			public static implicit operator ReadOnlySpan<T>(MySpan<T> s) => (Span<T>)s;
		}

		[DllImport("PPchatParsing.dll")]
		static extern MySpan<MySpan<byte>> GetTokensRangeImplementation(MySpan<byte> input);

		public static unsafe Span<MySpan<byte>> GetTokensRange(string input)
		{
			var bytes = Encoding.ASCII.GetBytes(input);
			MySpan<MySpan<byte>> s;
			fixed (byte* ptr = bytes)
			{
				s = GetTokensRangeImplementation(new MySpan<byte>(ptr, ptr + input.Length));
			}
			return s;
		}

		public static string MakeString(MySpan<byte> token)
			=> Encoding.ASCII.GetString(token);

		public static string MakeTailString(string s, Span<byte> token)
			=> s.Substring(token.Length);

		public static string[] MakeStrings(Span<MySpan<byte>> tokens)
		{
			var arr = new string[tokens.Length];

			for (int i = 0; i != arr.Length; ++i)
				arr[i] = MakeString(tokens[i]);

			return arr;
		}

		public static void SetEncoding()
		{
			Console.InputEncoding = Encoding.ASCII;
			Console.OutputEncoding = Console.InputEncoding;
		}
	}

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

			var commands = commandsSniffer.GetValue(NativeParsing.MakeString(tokens[0]));

			if (commands != null)
			{
				{
					var command = commands.GetIfOneLongArgument;
					if (command != null)
						return JustOneArgument(command, NativeParsing.MakeTailString(s, tokens[0]));
				}
				tokens = tokens.Slice(1);

				var commandsWithRightArgumentCount = commands.GetValue(tokens.Length);
				if (commandsWithRightArgumentCount != null)
					return (commandsWithRightArgumentCount, NativeParsing.MakeStrings(tokens));
				else
					return JustOneArgument(commandsSniffer.BadArgumentCountCommand, tokens.Length);
			}
			else
				return JustOneArgument(commandsSniffer.NotFoundCommand, s.TrimStart());
		}

		public (IInvoker<IApplication, object[]>, object[]) Parse(string input)
		{
			var (commands, arguments) = ParseImplementation(input);
			return (new EnumerableInvoker<IApplication, object[]>(commands), arguments);
		}
	}
}
