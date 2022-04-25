using System.Collections.Generic;
using UnityEngine;

namespace Marty
{
    public static class PoolingSystem
    {
        public static PoolManager poolManager = null;

        private static Dictionary<GameObject, GameObjectPool> pools = new Dictionary<GameObject, GameObjectPool>();
        private static Dictionary<GameObject, GameObjectPool> spawnedObjects = new Dictionary<GameObject, GameObjectPool>();

        public static GameObjectPool CreatePool(GameObject template, int maxSize = 0,
            int preSpawnCount = 0, Transform parent = null)
        {
            CheckInit();
            if (!pools.ContainsKey(template))
            {
                GameObjectPool pool = new GameObjectPool(template, maxSize, parent);
                pool.PreSpawnObjects(preSpawnCount);
                pools.Add(template, pool);
                return pool;
            }
            return null;
        }

        public static ComponentPool<T> CreatePool<T>(T template, int maxSize = 0,
            int preSpawnCount = 0, Transform parent = null) where T : Component
        {
            CheckInit();
            if (!pools.ContainsKey(template.gameObject))
            {
                ComponentPool<T> pool = new ComponentPool<T>(template, maxSize, parent);
                pool.PreSpawnObjects(preSpawnCount);
                pools.Add(template.gameObject, pool);
                return pool;
            }
            return null;
        }

        private static void CheckInit()
        {
            if (poolManager == null)
            {
                PoolManager.Create();
            }
        }

        public static GameObject GetObjectFromPool(GameObject template)
        {
            if (pools.TryGetValue(template, out GameObjectPool pool))
            {
                GameObject obj = pool.GetObjectFromPool();
                if (!spawnedObjects.ContainsKey(obj))
                {
                    spawnedObjects.Add(obj, pool);
                }
                return obj;
            }
            return null;
        }

        public static T GetObjectFromPool<T>(T template) where T : Component
        {
            GameObject obj = GetObjectFromPool(template.gameObject);
            if (obj != null)
            {
                return obj.GetComponent<T>();
            }
            return null;
        }

        public static bool ReturnObjectToPool(GameObject obj)
        {
            if (spawnedObjects.TryGetValue(obj, out GameObjectPool pool))
            {
                if (!pool.ReturnObjectToPool(obj))
                {
                    spawnedObjects.Remove(obj);
                }
                return true;
            }
            return false;
        }

        public static bool ReturnObjectToPool<T>(T component) where T : Component
        {
            return ReturnObjectToPool(component.gameObject);
        }

        public static void DropPool(GameObject template)
        {
            if (pools.TryGetValue(template, out GameObjectPool pool))
            {
                List<GameObject> removeList = new List<GameObject>();
                foreach (KeyValuePair<GameObject, GameObjectPool> kv in spawnedObjects)
                {
                    if (kv.Value == pool)
                    {
                        removeList.Add(kv.Key);
                    }
                }
                foreach (GameObject key in removeList)
                {
                    spawnedObjects.Remove(key);
                }
                pool.Clear();
                pools.Remove(template);
            }
        }

        public static void Reset()
        {
            spawnedObjects.Clear();
            pools.Clear();
            poolManager = null;
        }
    }
}
