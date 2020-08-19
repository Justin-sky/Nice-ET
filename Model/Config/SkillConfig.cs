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
		public string _Name;
		public string _Description;
		public int _CoolTime;
		public int _CostSP;
		public float _AttackDistance;
		public float _AttackAngle;
		public string[] _AttackTargetTags;
		public string[] _ImpactType;
		public int _NextBattlerId;
		public float _AtkRatio;
		public float _DurationTime;
		public float _AtkInterval;
		public string _SkillPrefab;
		public string _AnimationName;
		public string _HitFxPrefab;
		public int _Level;
		public int _AttackType;
		public int _SelectorType;
	}
}
