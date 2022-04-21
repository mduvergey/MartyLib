using UnityEngine;

namespace Marty
{
    public class PoolManager : MonoBehaviour
    {
        private void Awake()
        {
            if (PoolingSystem.poolManager == null)
            {
                PoolingSystem.poolManager = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            PoolingSystem.Reset();
        }

        public static void Create()
        {
            if (PoolingSystem.poolManager == null)
            {
                GameObject go = new GameObject("[PoolManager]");
                PoolingSystem.poolManager = go.AddComponent<PoolManager>();
            }
        }
    }
}
