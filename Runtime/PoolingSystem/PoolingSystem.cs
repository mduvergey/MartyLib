using System.Collections.Generic;
using UnityEngine;

namespace Marty
{
    public static class PoolingSystem
    {
        public static PoolManager poolManager = null;

        private static Dictionary<GameObject, GameObjectPool> pools = new Dictionary<GameObject, GameObjectPool>();
        private static Dictionary<GameObject, GameObjectPool> activeObjects = new Dictionary<GameObject, GameObjectPool>();

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
                activeObjects.Add(obj, pool);
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
            if (activeObjects.TryGetValue(obj, out GameObjectPool pool))
            {
                pool.ReturnObjectToPool(obj);
                activeObjects.Remove(obj);
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
                foreach (KeyValuePair<GameObject, GameObjectPool> kv in activeObjects)
                {
                    if (kv.Value == pool)
                    {
                        removeList.Add(kv.Key);
                    }
                }
                foreach (GameObject key in removeList)
                {
                    activeObjects.Remove(key);
                }
                pool.Clear();
                pools.Remove(template);
            }
        }

        public static void Reset()
        {
            activeObjects.Clear();
            pools.Clear();
            poolManager = null;
        }

        [Oddworm.Framework.PlayModeInspectorMethod]
        private static void PlayModeInspectorMethod()
        {
        #if UNITY_EDITOR
            foreach (KeyValuePair<GameObject, GameObjectPool> kv in pools)
            {
                UnityEditor.EditorGUILayout.BeginHorizontal();

                UnityEditor.EditorGUILayout.ObjectField(kv.Key, typeof(GameObject), false);

                int activeCount = 0;
                foreach (KeyValuePair<GameObject, GameObjectPool> kv2 in activeObjects)
                {
                    if (kv2.Value == kv.Value)
                    {
                        activeCount++;
                    }
                }

                float progress = (float)activeCount / (activeCount + kv.Value.GetPoolSize());

                UnityEditor.EditorGUI.ProgressBar(GUILayoutUtility.GetRect(100, GUI.skin.button.CalcHeight(new GUIContent("Wg"), 100)),
                    progress,
                    string.Format("{0} / {1} ({2:F2}%)", activeCount, activeCount + kv.Value.GetPoolSize(), progress));

                UnityEditor.EditorGUILayout.EndHorizontal();
                GUILayout.Space(2);
            }
        #endif // UNITY_EDITOR
        }
    }
}
