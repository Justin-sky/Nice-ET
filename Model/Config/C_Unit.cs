using MongoDB.Bson.Serialization.Attributes;

namespace NiceET
{
	[Config]
	public partial class C_UnitCategory : ACategory<C_Unit>
	{
		public static C_UnitCategory Instance;
		public C_UnitCategory()
		{
			Instance = this;
		}
	}

	public partial class C_Unit: IConfig
	{
		[BsonId]
		public long Id { get; set; }
		public float BaseATK;
		public float SP;
		public float HP;
		public float AttackDistance;
		public float AttackInterval;
	}
}
