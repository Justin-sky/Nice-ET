using System;
using System.Collections.Generic;

namespace NiceET
{
	public class OpcodeTypeComponent : Entity
	{
		public static OpcodeTypeComponent Instance;

		private readonly DoubleMap<ushort, Type> opcodeTypes = new DoubleMap<ushort, Type>();

		private readonly Dictionary<ushort, object> typeMessages = new Dictionary<ushort, object>();

		public void Load()
		{
			this.opcodeTypes.Clear();
			this.typeMessages.Clear();

			HashSet<Type> types = Game.EventSystem.GetTypes(typeof(MessageAttribute));
			foreach (Type type in types)
			{
				object[] attrs = type.GetCustomAttributes(typeof(MessageAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}

				MessageAttribute messageAttribute = attrs[0] as MessageAttribute;
				if (messageAttribute == null)
				{
					continue;
				}

				this.opcodeTypes.Add(messageAttribute.Opcode, type);
				this.typeMessages.Add(messageAttribute.Opcode, Activator.CreateInstance(type));
			}
		}

		public ushort GetOpcode(Type type)
		{
			return this.opcodeTypes.GetKeyByValue(type);
		}

		public Type GetType(ushort opcode)
		{
			return this.opcodeTypes.GetValueByKey(opcode);
		}

		// 客户端为了0GC需要消息池，服务端消息需要跨协程不需要消息池
		public object GetInstance(ushort opcode)
		{
			Type type = this.GetType(opcode);
			if (type == null)
			{
				// 服务端因为有人探测端口，有可能会走到这一步，如果找不到opcode，抛异常
				throw new Exception($"not found opcode: {opcode}");
			}
			return Activator.CreateInstance(type);
		}
	}
}