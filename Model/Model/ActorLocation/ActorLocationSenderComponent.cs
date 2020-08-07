using System;

namespace NiceET
{
    public class ActorLocationSenderComponent : Entity
    {
        [NoMemoryCheck]
        public static long TIMEOUT_TIME = 10 * 1000;

        public static ActorLocationSenderComponent Instance { get; set; }

        public long CheckTimer;
    }
}