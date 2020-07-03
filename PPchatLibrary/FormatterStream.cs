using System.Runtime.Serialization;
using System.IO;

namespace PPchatLibrary
{
	public class FormatterStream<Formatter, T> : IReaderWriter<T>
		where Formatter : IFormatter, new()
		where T : class
	{
		readonly Stream stream;
		readonly IFormatter formatter;

		public FormatterStream(Stream stream)
		{
			this.stream = stream;
			formatter = new Formatter();
		}

		public void Write(T t)
		{
			formatter.Serialize(stream, t);
		}
		public T Read()
		{
			return (T)formatter.Deserialize(stream);
		}
	}
}
