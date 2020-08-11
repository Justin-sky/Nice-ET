using MongoDB.Driver;
using System;

namespace NiceET
{
    public class DBComponentAwakeSystem : AwakeSystem<DBComponent, string, string>
	{
		public override void Awake(DBComponent self, string dbConnection, string dbName)
		{
			self.mongoClient = new MongoClient(dbConnection);
			self.database = self.mongoClient.GetDatabase(dbName);

			self.Transfers.Clear();
			foreach (Type type in Game.EventSystem.GetTypes())
			{
				if (type == typeof(IDBCollection))
				{
					continue;
				}
				if (!typeof(IDBCollection).IsAssignableFrom(type))
				{
					continue;
				}
				self.Transfers.Add(type.Name);
			}

			DBComponent.Instance = self;
		}
	}

	public class DBComponentDestroySystem : DestroySystem<DBComponent>
	{
		public override void Destroy(DBComponent self)
		{
			DBComponent.Instance = null;
			self.Transfers.Clear();
		}
	}

	

}
