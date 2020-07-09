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
		static extern MySpan<MySpan<char>> GetTokensRangeImplementation(MySpan<char> input);

		public static unsafe Span<MySpan<char>> GetTokensRange(string input)
		{
			MySpan<MySpan<char>> s;
			fixed (char* ptr = input)
			{
				s = GetTokensRangeImplementation(new MySpan<char>(ptr, ptr + input.Length));
			}
			return s;
		}

		public static void SetEncoding()
		{
			Console.InputEncoding = Encoding.ASCII;
			Console.OutputEncoding = Console.InputEncoding;
		}

		public unsafe static long GetOffsetFromStart(string s, ReadOnlySpan<char> span)
		{
			fixed (char* string_ptr = s, span_ptr = span)
			{
				return span_ptr - string_ptr;
			}
		}

		public static ReadOnlyMemory<char> GetMemory(string s, ReadOnlySpan<char> token)
			=> s.AsMemory().Slice((int)GetOffsetFromStart(s, token), token.Length);

		public static ReadOnlyMemory<char> GetTailMemory(string s, ReadOnlySpan<char> token)
			=> s.AsMemory().Slice(token.Length);

		public static object[] MakeMemories(string s, Span<MySpan<char>> tokens)
		{
			var arr = new object[tokens.Length];

			for (int i = 0; i != arr.Length; ++i)
				arr[i] = GetMemory(s, tokens[i]);

			return arr;
		}

		[DllImport("PPchatParsing.dll")]
		public static extern void ReleaseResources();
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
