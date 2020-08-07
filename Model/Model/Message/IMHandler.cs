using System;

namespace NiceET
{
	public interface IMHandler
	{
		ETVoid Handle(Session session, object message);
		Type GetMessageType();
	}
}