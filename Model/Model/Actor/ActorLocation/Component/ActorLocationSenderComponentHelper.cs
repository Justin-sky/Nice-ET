using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
    public static class ActorLocationSenderComponentHelper
    {
        public static void Check(this ActorLocationSenderComponent self, bool isTimeOut)
        {
            using (ListComponent<long> list = EntityFactory.Create<ListComponent<long>>(self.Domain))
            {
                long timeNow = TimeHelper.Now();
                foreach ((long key, Entity value) in self.Children)
                {
                    ActorLocationSender actorLocationMessageSender = (ActorLocationSender)value;

                    if (timeNow > actorLocationMessageSender.LastSendOrRecvTime + ActorLocationSenderComponent.TIMEOUT_TIME)
                    {
                        list.List.Add(key);
                    }
                }

                foreach (long id in list.List)
                {
                    self.Remove(id);
                }
            }
        }

        private static ActorLocationSender Get(this ActorLocationSenderComponent self, long id)
        {
            if (id == 0)
            {
                throw new Exception($"actor id is 0");
            }
            if (self.Children.TryGetValue(id, out Entity actorLocationSender))
            {
                return (ActorLocationSender)actorLocationSender;
            }

            actorLocationSender = EntityFactory.CreateWithId<ActorLocationSender>(self.Domain, id);
            actorLocationSender.Parent = self;
            return (ActorLocationSender)actorLocationSender;
        }

        private static void Remove(this ActorLocationSenderComponent self, long id)
        {
            if (!self.Children.TryGetValue(id, out Entity actorMessageSender))
            {
                return;
            }
            actorMessageSender.Dispose();
        }

        public static void Send(this ActorLocationSenderComponent self, long entityId, IActorLocationMessage message)
        {
            ActorLocationSender actorLocationSender = self.Get(entityId);
            actorLocationSender.Send(message);
        }

        public static async ETTask<IActorResponse> Call(this ActorLocationSenderComponent self, long entityId, IActorLocationRequest message)
        {
            ActorLocationSender actorLocationSender = self.Get(entityId);
            return await actorLocationSender.Call(message);
        }
    }
}
