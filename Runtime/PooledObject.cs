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
            ObjectPool.ReturnObjectToPool(gameObject);
        }
    }
}
