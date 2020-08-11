namespace NiceET
{


	public sealed class Player : Entity
	{
		public string Account { get; private set; }

		public long UnitId { get; set; }

		public void Awake(string account)
		{
			this.Account = account;
		}
	}
}