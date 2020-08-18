using MongoDB.Bson.Serialization.Attributes;

namespace NiceET
{
	[Config]
	public partial class SkillConfigCategory : ACategory<SkillConfig>
	{
		public static SkillConfigCategory Instance;
		public SkillConfigCategory()
		{
			Instance = this;
		}
	}

	public partial class SkillConfig: IConfig
	{
		[BsonId]
		public long Id { get; set; }
		public string Name;
		public string Description;
		public int CoolTime;
		public int CostSP;
		public float AttackDistance;
		public float AttackAngle;
		public string[] AttackTargetTags;
		public string[] ImpactType;
		public int NextBattlerId;
		public float AtkRatio;
		public float DurationTime;
		public float AtkInterval;
		public string SkillPrefab;
		public string AnimationName;
		public string HitFxPrefab;
		public int Level;
		public int AttackType;
		public int SelectorType;
	}
}
