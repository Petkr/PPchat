using System.IO;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;

namespace PPchatLibrary
{
	public class SimpleSerializerEndOfStreamException : Exception
	{ }

	public class SimpleSerializerStream : IDisposable,
		IReaderWriter<int>,
		IReaderWriter<string>,
		IReaderWriter<IPAddress>,
		IReader<Memory<char>>,
		IWriter<ReadOnlyMemory<char>>
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



		unsafe void WriteUnmanaged<T>(T t)
			where T : unmanaged
		{
			Span<byte> span = stackalloc byte[sizeof(int)];
			MemoryMarshal.Write(span, ref t);
			Stream.Write(span);
		}
		unsafe T ReadUnmanaged<T>()
			where T : unmanaged
		{
			Span<byte> span = stackalloc byte[sizeof(T)];
			if (Stream.Read(span) != sizeof(T))
				throw new SimpleSerializerEndOfStreamException();
			return MemoryMarshal.Read<T>(span);
		}



		unsafe void WriteUnmanagedMany<T>(ReadOnlySpan<T> span)
			where T : unmanaged
		{
			WriteUnmanaged(span.Length);
			WriteRaw(MemoryMarshal.AsBytes(span));
		}
		unsafe T[] ReadUnmanagedMany<T>()
			where T : unmanaged
		{
			T[] array = new T[ReadUnmanaged<int>()];
			ReadRaw(MemoryMarshal.AsBytes(array.AsSpan()));
			return array;
		}



		void WriteRaw(ReadOnlySpan<byte> bytes)
			=> Stream.Write(bytes);
		void ReadRaw(Span<byte> bytes)
		{
			if (Stream.Read(bytes) != bytes.Length)
				throw new SimpleSerializerEndOfStreamException();
		}



		public void Write(int t)
			=> WriteUnmanaged(t);
		int IReader<int>.Read()
			=> ReadUnmanaged<int>();

		public void Write(string t)
			=> WriteUnmanagedMany(t.AsSpan());
		string IReader<string>.Read()
			=> Encoding.ASCII.GetString(MemoryMarshal.AsBytes(ReadUnmanagedMany<char>().AsSpan()));

		IPAddress IReader<IPAddress>.Read()
			=> new IPAddress(ReadUnmanagedMany<byte>());
		public void Write(IPAddress t)
		{
			Span<byte> bytes = stackalloc byte[16];
			if (t.TryWriteBytes(bytes, out var writtenLength))
			{
				bytes = bytes.Slice(0, writtenLength);
				WriteUnmanagedMany<byte>(bytes);
			}
			else
				throw new Exception();
		}

		Memory<char> IReader<Memory<char>>.Read()
			=> ReadUnmanagedMany<char>().AsMemory();

		public void Write(ReadOnlyMemory<char> t)
			=> WriteUnmanagedMany(t.Span);
	}
}
