using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public interface ITimer
	{
		void Run(bool isTimeout);
	}
}
