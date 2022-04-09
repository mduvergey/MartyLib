using UnityEngine;

namespace Marty
{
    [DisallowMultipleComponent]
    public class PooledObject : MonoBehaviour
    {
        public ObjectPool ObjectPool { private get; set; }

        [SerializeField] private bool returnToPoolOnDisable = false;

        private void OnDisable()
        {
            if (returnToPoolOnDisable)
            {
                ReturnToPool();
            }
        }

        public void ReturnToPool()
        {
            if (ObjectPool != null)
            {
                ObjectPool.ReturnObjectToPool(gameObject);
            }
            else
            {
                Debug.LogError("Object pool not set. Unable to return pooled object.");
            }
        }
    }
}
