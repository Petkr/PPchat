using System.IO;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Collections.Generic;

namespace PPchatLibrary
{
	public class SimpleSerializerEndOfStreamException : Exception
	{ }

	public class SimpleSerializerStream : IDisposable,
		IReaderWriter<int>,
		IReaderWriter<Memory<byte>>,
		IReaderWriter<byte[]>,
		IReaderWriter<string>,
		IReaderWriter<IPAddress>
	{
		readonly Stream Stream;

		public SimpleSerializerStream(Stream stream)
			=> Stream = stream;

		public void Dispose()
			=> Stream.Dispose();



		public T Read<T>() => ((IReader<T>)this).Read();
		public void Write<T>(T t) => ((IWriter<T>)this).Write(t);



		public void Write<T, U>((T, U) pair)
		{
			Write(pair.Item1);
			Write(pair.Item2);
		}
		public (T, U) ReadPair<T, U>()
		{
			var t = Read<T>();
			var u = Read<U>();
			return (t, u);
		}

		public void Write<Key, Value>(KeyValuePair<Key, Value> pair)
			=> Write((pair.Key, pair.Value));
		public KeyValuePair<Key, Value> ReadKeyValuePair<Key, Value>()
		{
			var (key, value) = ReadPair<Key, Value>();
			return new KeyValuePair<Key, Value>(key, value);
		}



		void ReadBytes(Span<byte> bytes)
		{
			if (Stream.Read(bytes) != bytes.Length)
				throw new SimpleSerializerEndOfStreamException();
		}
		void ReadBytes(byte[] bytes)
			=> ReadBytes(bytes.AsSpan());
		void ReadBytes(Memory<byte> bytes)
			=> ReadBytes(bytes.Span);

		void WriteBytes(ReadOnlySpan<byte> bytes)
			=> Stream.Write(bytes);



		public void Write(Memory<byte> bytes)
			=> Write(bytes.Span);
		Memory<byte> IReader<Memory<byte>>.Read()
			=> Read<byte[]>().AsMemory();

		public void Write(Span<byte> bytes)
		{
			Write(bytes.Length);
			WriteBytes(bytes);
		}
		public Span<byte> ReadSpan()
			=> Read<Memory<byte>>().Span;

		public void Write(byte[] bytes)
			=> Write(bytes.AsSpan());
		byte[] IReader<byte[]>.Read()
		{
			var length = Read<int>();
			var bytes = new byte[length];
			ReadBytes(bytes);
			return bytes;
		}

		public void Write(int t)
		{
			Span<byte> span = stackalloc byte[sizeof(int)];
			MemoryMarshal.Write(span, ref t);
			Stream.Write(span);
		}
		int IReader<int>.Read()
		{
			Span<byte> span = stackalloc byte[sizeof(int)];
			if (Stream.Read(span) != sizeof(int))
				throw new SimpleSerializerEndOfStreamException();
			return MemoryMarshal.Read<int>(span);
		}

		public void Write(string t)
			=> Write(Encoding.ASCII.GetBytes(t));
		string IReader<string>.Read()
			=> Encoding.ASCII.GetString(Read<byte[]>());


		IPAddress IReader<IPAddress>.Read()
			=> new IPAddress(ReadSpan());
		public void Write(IPAddress t)
			=> Write(t.GetAddressBytes());
	}
}
