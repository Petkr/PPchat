using System;

namespace PPchatLibrary
{
	[Serializable]
	public readonly struct EndPacket : IPacket
	{
		public readonly string Reason;

		public EndPacket(string reason)
		{
			Reason = reason;
		}
	}
}
