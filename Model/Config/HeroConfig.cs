using MongoDB.Bson.Serialization.Attributes;

namespace NiceET
{
	[Config]
	public partial class HeroConfigCategory : ACategory<HeroConfig>
	{
		public static HeroConfigCategory Instance;
		public HeroConfigCategory()
		{
			Instance = this;
		}
	}

	public partial class HeroConfig: IConfig
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
