using System;
using System.Collections.Generic;
using System.Net;


namespace NiceET
{
	[MessageHandler]
	public class C2R_LoginHandler : AMRpcHandler<C2R_Login, R2C_Login>
	{
		protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response, Action reply)
		{
			// 随机分配一个Gate
			StartSceneConfig config = RealmGateAddressHelper.GetGate(session.DomainZone());
			Log.Debug($"gate address: {MongoHelper.ToJson(config)}");

			string accountName = request.Account;
			string password = request.Password;
			//验证账号密码
			List<Account> list = await DBComponent.Instance.Query<Account>(
				a => a.AccountName.Equals(accountName) && a.Password.Equals(password)
				);
			Account account;
			if (list == null || list.Count == 0)
            {
				account = EntityFactory.Create<Account, string, string>(Game.Scene, accountName, password);
				await DBComponent.Instance.Save(account);
            }
            else
            {
				account = list[0];
			}
			
			// 向gate请求一个key,客户端可以拿着这个key连接gate
			G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await ActorMessageSenderComponent.Instance.Call(
				config.SceneId, new R2G_GetLoginKey() { Account = account.Id.ToString() });

			response.Address = config.OuterAddress;
			response.Key = g2RGetLoginKey.Key;
			response.GateId = g2RGetLoginKey.GateId;
			reply();
		}
	}
}