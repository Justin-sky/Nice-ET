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
		public float _BaseATK;
		public float _SP;
		public float _HP;
		public float _AttackDistance;
		public float _AttackInterval;
	}
}
