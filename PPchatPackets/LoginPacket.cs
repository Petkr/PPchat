using System;
using PPchatLibrary;

namespace PPchatPackets
{
	[Serializable]
	public readonly struct LoginPacket : IPacket
	{
		public readonly string Username;

		public LoginPacket(string username)
		{
			Username = username;
		}
	}
}
