using System.IO;
using System.Reflection;

namespace NiceET
{
	public static class DllHelper
	{
		public static Assembly GetHotfixAssembly()
		{
			byte[] dllBytes = File.ReadAllBytes("./Hotfix.dll");
			byte[] pdbBytes = File.ReadAllBytes("./Hotfix.pdb");
			Assembly assembly = Assembly.Load(dllBytes, pdbBytes);
			return assembly;
		}
	}
}
