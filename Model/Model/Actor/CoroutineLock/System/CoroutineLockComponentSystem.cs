using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
    public class CoroutineLockComponentSystem : AwakeSystem<CoroutineLockComponent>
    {
        public override void Awake(CoroutineLockComponent self)
        {
            self.Awake();
        }
    }
}
