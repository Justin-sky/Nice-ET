using System;
using System.Collections.Generic;

namespace NiceET
{
    public static class ConfigComponentHelper
	{
		public static void Awake(this ConfigComponent self)
		{
			self.Load();
		}

		public static void Load(this ConfigComponent self)
		{
			self.AllConfig.Clear();
			HashSet<Type> types = Game.EventSystem.GetTypes(typeof(ConfigAttribute));

			foreach (Type type in types)
			{
				object obj = Activator.CreateInstance(type);

				ACategory iCategory = obj as ACategory;
				if (iCategory == null)
				{
					throw new Exception($"class: {type.Name} not inherit from ACategory");
				}
				iCategory.BeginInit();
				iCategory.EndInit();

				self.AllConfig[iCategory.ConfigType] = iCategory;
			}
		}
	}
}
