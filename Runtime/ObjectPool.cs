using System.Collections.Generic;
using UnityEngine;

namespace Marty
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab = null;
        [Tooltip("Will be adjusted if value is greater than Max Pool Size.")]
        [SerializeField, Min(0)] private int initialPoolSize = 10;
        [Tooltip("Set value to 0 to disable size limit.")]
        [SerializeField, Min(0)] private int maxPoolSize = 0;
        [Space]
        [Tooltip("Optional transform that will be used as parent for pooled objects instead of the transform of this object.")]
        [SerializeField] private Transform parentTransform = null;
        [Tooltip("Ensure a PooledObject component is present on every pooled object.")]
        [SerializeField] private bool addPooledObjectComponent = false;

        private Stack<GameObject> pool = new Stack<GameObject>();

        private void Awake()
        {
            if ((maxPoolSize > 0) && (initialPoolSize > maxPoolSize))
            {
                initialPoolSize = maxPoolSize;
            }

            if (prefab != null)
            {
                for (int i = 0; i < initialPoolSize; i++)
                {
                    GameObject go = SpawnObject();
                    go.SetActive(false);
                    pool.Push(go);
                }
            }
            else
            {
                Debug.LogError("Missing prefab. Object pool will not be able to spawn objects.");
            }
        }

        public GameObject GetObjectFromPool()
        {
            GameObject go;
            if (pool.Count > 0)
            {
                go = pool.Pop();
                go.SetActive(true);
            }
            else
            {
                go = SpawnObject();
            }
            return go;
        }

        public void ReturnObjectToPool(GameObject go)
        {
            if ((maxPoolSize > 0) && (pool.Count == maxPoolSize))
            {
                Destroy(go);
            }
            else if (!pool.Contains(go))
            {
                go.SetActive(false);
                pool.Push(go);
            }
        }

        private GameObject SpawnObject()
        {
            GameObject go;

            if (parentTransform != null)
            {
                go = Instantiate(prefab, parentTransform);
            }
            else
            {
                go = Instantiate(prefab, transform);
            }

            if (go.TryGetComponent<PooledObject>(out PooledObject pooledObject))
            {
                pooledObject.ObjectPool = this;
            }
            else if (addPooledObjectComponent)
            {
                go.AddComponent<PooledObject>().ObjectPool = this;
            }

            return go;
        }
    }
}
