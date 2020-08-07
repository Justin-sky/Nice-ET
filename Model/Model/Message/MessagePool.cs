using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class MessagePool
	{
		public static MessagePool Instance { get; } = new MessagePool();


		public object Fetch(Type type)
		{
			return Activator.CreateInstance(type);
		}

		public T Fetch<T>() where T : class
		{
			T t = (T)this.Fetch(typeof(T));
			return t;
		}

		public void Recycle(object obj)
		{

		}
	}
}
