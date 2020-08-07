using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
    public class ConsoleComponentAwakeSystem : StartSystem<ConsoleComponent>
    {
        public override void Start(ConsoleComponent self)
        {
            self.Start().Coroutine();
        }
    }
}
