using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class NumericChangeEvent_NotifyWatcher : AEvent<EventType.NumbericChange>
	{
		public override async ETTask Run(EventType.NumbericChange args)
		{
			NumericWatcherComponent.Instance.Run(args.NumericType, args.Parent.Id, args.New);
		}
	}
}
