using MongoDB.Bson.Serialization.Attributes;

namespace NiceET
{
	[Config]
	public partial class C_SkillDataCategory : ACategory<C_SkillData>
	{
		public static C_SkillDataCategory Instance;
		public C_SkillDataCategory()
		{
			Instance = this;
		}
	}

	public partial class C_SkillData: IConfig
	{
		[BsonId]
		public long Id { get; set; }
		public int SkillID;
		public string Name;
		public string Description;
		public int CoolTime;
		public int CostSP;
		public float AttackDistance;
		public float AttackAngle;
		public string[] AttackTargetTags;
		public string[] ImpactType;
		public int NextSkillId;
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
