using System;
using PPnetwork;

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
