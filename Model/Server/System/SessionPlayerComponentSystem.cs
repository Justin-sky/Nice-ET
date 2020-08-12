

namespace NiceET
{
	public class SessionPlayerComponentSystem : DestroySystem<SessionPlayerComponent>
	{
		public override void Destroy(SessionPlayerComponent self)
		{

			
			self.Domain.GetComponent<PlayerComponent>()?.Remove(self.Player.Id);

			// 发送断线消息
			//ActorLocationSenderComponent.Instance.Send(self.Player.UnitId, new G2M_SessionDisconnect());

		}
	}
}