

namespace NiceET
{
	[ActorMessageHandler]
	public class G2M_SessionDisconnectHandler : AMActorLocationHandler<Unit, G2M_SessionDisconnect>
	{
		protected override async ETTask Run(Unit unit, G2M_SessionDisconnect message)
		{
			Log.Debug("客户端下线，通知Map下线...");
			unit.GetComponent<UnitGateComponent>().IsDisconnect = true;
			await ETTask.CompletedTask;
		}
	}
}