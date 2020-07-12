using System;
using PPchatLibrary;

namespace PPchatPackets
{
	[Serializable]
	public readonly struct MessageForClientPacket : IPacket
	{
		public readonly string Message;

		public MessageForClientPacket(string message)
		{
			Message = message;
		}
	}
}
