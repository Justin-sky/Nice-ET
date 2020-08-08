namespace NiceET
{
    public class LockInfo : Entity
	{
		public long LockInstanceId;

		public CoroutineLock CoroutineLock;

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();

			this.CoroutineLock.Dispose();
			this.CoroutineLock = null;
			LockInstanceId = 0;
		}
	}
}
