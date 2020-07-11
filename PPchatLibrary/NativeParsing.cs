using System;
using System.Text;
using System.Runtime.InteropServices;

namespace PPchatLibrary
{
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
}
