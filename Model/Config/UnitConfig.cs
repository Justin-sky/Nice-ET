using MongoDB.Bson.Serialization.Attributes;

namespace NiceET
{
	[Config]
	public partial class UnitConfigCategory : ACategory<UnitConfig>
	{
		public static UnitConfigCategory Instance;
		public UnitConfigCategory()
		{
			Instance = this;
		}
	}

	public partial class UnitConfig: IConfig
	{
		[BsonId]
		public long Id { get; set; }
		public string _Name;
		public string _Desc;
		public int _Position;
		public int _Height;
		public int _Weight;
	}
}
