namespace NiceET
{


	public class UnitGateComponent : Entity, ISerializeToEntity
	{
		public long GateSessionActorId;

		public bool IsDisconnect;

		public void Awake(long gateSessionId)
		{
			this.GateSessionActorId = gateSessionId;
		}
	}
}