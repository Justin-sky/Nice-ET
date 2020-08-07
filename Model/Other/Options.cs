using CommandLine;

namespace NiceET
{
	public class Options : Entity
	{
		[Option("Process", Required = false, Default = 1)]
		public int Process { get; set; }
	}
}