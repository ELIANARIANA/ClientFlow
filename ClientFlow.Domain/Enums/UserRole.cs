namespace ClientFlow.Domain.Enums
{
	[Flags]
	public enum UserRole
	{
		None   = 0,
		User    = 1 << 0, // 1
		Admin   = 2 << 1, // 2
		Manager = 3 << 2, // 4
	}
}
