using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class OnceTimer : Entity, ITimer
	{
		public Action<bool> Callback { get; set; }

		public void Run(bool isTimeout)
		{
			try
			{
				this.Callback.Invoke(isTimeout);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}
