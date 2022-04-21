using UnityEngine;

namespace Marty
{
    public static class PoolingSystemExtensions
    {
        public static GameObjectPool CreatePool(this GameObject template,
            int maxSize = 0, int preSpawnCount = 0, Transform parent = null)
        {
            return PoolingSystem.CreatePool(template, maxSize, preSpawnCount, parent);
        }

        public static ComponentPool<T> CreatePool<T>(this T template,
            int maxSize = 0, int preSpawnCount = 0, Transform parent = null) where T : Component
        {
            return PoolingSystem.CreatePool(template, maxSize, preSpawnCount, parent);
        }

        public static GameObject GetObjectFromPool(this GameObject template)
        {
            return PoolingSystem.GetObjectFromPool(template);
        }

        public static T GetObjectFromPool<T>(this T template) where T : Component
        {
            return PoolingSystem.GetObjectFromPool(template);
        }

        public static bool ReturnToPool(this GameObject obj)
        {
            return PoolingSystem.ReturnObjectToPool(obj);
        }

        public static bool ReturnToPool<T>(this T component) where T : Component
        {
            return PoolingSystem.ReturnObjectToPool(component.gameObject);
        }

        public static void DropPool(this GameObject template)
        {
            PoolingSystem.DropPool(template);
        }

        public static void DropPool<T>(this T template) where T : Component
        {
            PoolingSystem.DropPool(template.gameObject);
        }
    }
}
