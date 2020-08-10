namespace NiceET
{
    public class CoroutineLockSystem : AwakeSystem<CoroutineLock, CoroutineLockType, long>
    {
        public override void Awake(CoroutineLock self, CoroutineLockType coroutineLockType, long key)
        {
            self.Awake(coroutineLockType, key);
        }
    }
}
