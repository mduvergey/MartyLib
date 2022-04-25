using System;
using System.Collections.Generic;

namespace Marty
{
    public class ObjectPool<T>
    {
        protected Func<T> createFunc;
        protected Action<T> onGet;
        protected Action<T> onRecycle;
        protected Action<T> onReturn;
        protected Action<T> onDestroy;

        private Stack<T> pool;
        private T template;
        private int maxPoolSize;

        public ObjectPool(T template,
            Func<T> createFunc,
            int maxPoolSize = 0,
            Action<T> onRecycle = null,
            Action<T> onGet = null,
            Action<T> onReturn = null,
            Action<T> onDestroy = null)
        {
            pool = new Stack<T>();
            this.template = template;
            this.createFunc = createFunc;
            this.maxPoolSize = maxPoolSize;
            this.onRecycle = onRecycle;
            this.onGet = onGet;
            this.onReturn = onReturn;
            this.onDestroy = onDestroy;
        }

        public void PreSpawnObjects(int count)
        {
            if ((maxPoolSize > 0) && (count + pool.Count > maxPoolSize))
            {
                count = maxPoolSize - pool.Count;
            }

            for (int i = 0; i < count; i++)
            {
                T obj = createFunc();
                pool.Push(obj);
            }
        }

        public T GetObjectFromPool()
        {
            T obj;

            if (pool.Count > 0)
            {
                obj = pool.Pop();
                onRecycle?.Invoke(obj);
            }
            else
            {
                obj = createFunc();
            }

            onGet?.Invoke(obj);
            return obj;
        }

        public bool ReturnObjectToPool(T obj)
        {
            if ((maxPoolSize > 0) && (pool.Count == maxPoolSize))
            {
                onDestroy?.Invoke(obj);
                return false;
            }

            onReturn?.Invoke(obj);
            pool.Push(obj);
            return true;
        }

        public void Clear()
        {
            if (onDestroy != null)
            {
                foreach (T obj in pool)
                {
                    onDestroy.Invoke(obj);
                }
            }
            pool.Clear();
        }
    }
}
