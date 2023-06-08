namespace Cmune.DataCenter.Common.Entities
{
	public enum MemberAuthenticationResult
	{
		Ok,
		InvalidData,
		InvalidName,
		InvalidEmail,
		InvalidPassword,
		IsBanned,
		InvalidHandle,
		InvalidEsns,
		InvalidCookie,
		IsIpBanned,
		UnknownError,
		NewUpdate
	}
}
