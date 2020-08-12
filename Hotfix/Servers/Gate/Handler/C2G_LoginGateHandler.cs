using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace NiceET
{
	[MessageHandler]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply)
		{
			Scene scene = Game.Scene.Get(request.GateId);
			if (scene == null)
			{
				return;
			}

			string account = scene.GetComponent<GateSessionKeyComponent>().Get(request.Key);
			if (account == null)
			{
				response.Error = ErrorCode.ERR_ConnectGateKeyError;
				response.Message = "Gate key验证失败!";
				reply();
				return;
			}

			List<Player> players =  await DBComponent.Instance.Query<Player>(
					p=> p.Account.Equals(account)
				);

			Player player ;
			if (players == null || players.Count == 0)
            {
				player = EntityFactory.Create<Player, string>(Game.Scene, account);
				await DBComponent.Instance.Save(player);
            }
            else
            {
				player = players[0];
            }

			scene.GetComponent<PlayerComponent>().Add(player);

			session.AddComponent<SessionPlayerComponent>().Player = player;
			session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);

			response.PlayerId = player.Id;
			reply();

			session.Send(new G2C_TestHotfixMessage() { Info = "recv hotfix message success" });
			await ETTask.CompletedTask;
		}
	}
}