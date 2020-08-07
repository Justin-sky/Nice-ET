using System;

namespace NiceET
{
    public class ETCancellationToken
    {
        private Action action;

        public void Register(Action callback)
        {
            this.action = callback;
        }

        public void Cancel()
        {
            action.Invoke();
        }
    }
}