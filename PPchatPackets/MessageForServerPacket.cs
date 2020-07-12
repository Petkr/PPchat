using System;
using PPchatLibrary;

namespace PPchatPackets
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
