using System;

namespace Marty
{
    public class DoOnce
    {
        private Action action;
        private bool done;

        public DoOnce(Action action)
        {
            this.action = action;
            done = false;
        }

        public void Invoke()
        {
            if (!done)
            {
                done = true;
                action?.Invoke();
            }
        }

        public void Reset()
        {
            done = false;
        }
    }
}
