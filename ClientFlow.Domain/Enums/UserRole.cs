namespace ClientFlow.Domain.Enums
{
	[Flags]
	public enum UserRole
	{
		None   = 0,
		User    = 1 << 0, // 1
		Admin   = 1 << 1, // 2
		Manager = 1 << 2, // 4
	}
}
