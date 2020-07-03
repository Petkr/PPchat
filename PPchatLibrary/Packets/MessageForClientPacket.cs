using System;

namespace PPchatLibrary
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
