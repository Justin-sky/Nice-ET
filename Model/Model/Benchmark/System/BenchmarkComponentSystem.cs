using System;
using System.Net;
using System.Threading.Tasks;

namespace NiceET
{
	public class BenchmarkComponentSystem : AwakeSystem<BenchmarkComponent, string>
	{
		public override void Awake(BenchmarkComponent self, string a)
		{
			self.Awake(a);
		}
	}

	
}
