using System;

namespace Marty
{
    public class DoN
    {
        private Action action;
        private int limit;
        private int invocationsLeft;

        public DoN(Action action, int limit)
        {
            this.action = action;
            this.limit = limit;
            invocationsLeft = limit;
        }

        public void Invoke()
        {
            if (invocationsLeft > 0)
            {
                invocationsLeft--;
                action?.Invoke();
            }
        }

        public void Reset()
        {
            invocationsLeft = limit;
        }
    }
}
