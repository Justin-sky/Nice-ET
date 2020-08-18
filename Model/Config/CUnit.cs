using MongoDB.Bson.Serialization.Attributes;

namespace NiceET
{
	[Config]
	public partial class CUnitCategory : ACategory<CUnit>
	{
		public static CUnitCategory Instance;
		public CUnitCategory()
		{
			Instance = this;
		}
	}

	public partial class CUnit: IConfig
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
