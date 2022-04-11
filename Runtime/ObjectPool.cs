using System.Collections.Generic;
using UnityEngine;

namespace Marty
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private Stack<T> pool;
        private T original;
        private int maxPoolSize;
        private System.Action<T> onCreate;
        private System.Action<T> onRecycle;
        private Transform parent;

        public ObjectPool(T original, int maxPoolSize = 0,
            System.Action<T> onCreate = null, System.Action<T> onRecycle = null, Transform parent = null)
        {
            pool = new Stack<T>();
            this.original = original;
            this.maxPoolSize = maxPoolSize;
            this.onCreate = onCreate;
            this.onRecycle = onRecycle;
            this.parent = parent;
        }

        public void PreSpawnObjects(int count)
        {
            if ((maxPoolSize > 0) && (count + pool.Count > maxPoolSize))
            {
                count = maxPoolSize - pool.Count;
            }

            for (int i = 0; i < count; i++)
            {
                T pooledObject = SpawnObject();
                pooledObject.gameObject.SetActive(false);
                pool.Push(pooledObject);
            }
        }

        public T GetObjectFromPool()
        {
            T pooledObject;
            if (pool.Count > 0)
            {
                pooledObject = pool.Pop();
                onRecycle?.Invoke(pooledObject);
                pooledObject.gameObject.SetActive(true);
            }
            else
            {
                pooledObject = SpawnObject();
            }
            return pooledObject;
        }

        public void ReturnObjectToPool(T pooledObject)
        {
            if ((maxPoolSize > 0) && (pool.Count == maxPoolSize))
            {
                Object.Destroy(pooledObject.gameObject);
            }
            else
            {
                pooledObject.gameObject.SetActive(false);
                pool.Push(pooledObject);
            }
        }

        private T SpawnObject()
        {
            GameObject go;

            if (parent != null)
            {
                go = Object.Instantiate(original.gameObject, parent);
            }
            else
            {
                go = Object.Instantiate(original.gameObject);
            }

            T obj = go.GetComponent<T>();
            onCreate?.Invoke(obj);
            return obj;
        }
    }
}
