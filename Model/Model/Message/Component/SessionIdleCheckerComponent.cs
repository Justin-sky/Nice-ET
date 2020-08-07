namespace NiceET
{
    public class SessionIdleCheckerComponent : Entity
    {
        public int CheckInterval;
        public int RecvMaxIdleTime;
        public int SendMaxIdleTime;
        public long RepeatedTimer;
    }
}