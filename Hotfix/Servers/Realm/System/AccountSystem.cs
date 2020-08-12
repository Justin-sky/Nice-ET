namespace NiceET
{
	public class AccountSystem : AwakeSystem<Account, string, string>
	{
		public override void Awake(Account self, string a, string b)
		{
			self.Awake(a,b);
		}
	}
}
