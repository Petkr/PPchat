using System;

namespace PPchatLibrary
{
	abstract class ParseException : Exception
	{
		public ParseException(string message)
			: base($"parsing exception: {message}")
		{}
	}
}
