using System;

namespace PPchatLibrary
{
	[Serializable]
	public readonly struct MessageForServerPacket : IPacket
	{
		public readonly string Message;

		public MessageForServerPacket(string message)
		{
			Message = message;
		}
	}
}
