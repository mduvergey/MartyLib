using UnityEngine;

namespace Marty
{
    public class ComponentPool<T> : ObjectPool<T> where T : Component
    {
        public ComponentPool(T template, int maxPoolSize = 0,
            System.Action<T> onCreate = null,
            System.Action<T> onRecycle = null,
            System.Action<T> onGet = null,
            System.Action<T> onReturn = null,
            System.Action<T> onDestroy = null,
            Transform parent = null) :
            base(template, null, maxPoolSize, onRecycle)
        {
            this.createFunc = () => {
                T obj;

                if (parent != null)
                {
                    obj = Object.Instantiate(template, parent);
                }
                else
                {
                    obj = Object.Instantiate(template);
                }

                onCreate?.Invoke(obj);
                obj.gameObject.SetActive(false);
                return obj;
            };

            this.onGet = obj => {
                onGet?.Invoke(obj);
                obj.gameObject.SetActive(true);
            };

            this.onReturn = obj => {
                onReturn?.Invoke(obj);
                obj.gameObject.SetActive(false);
            };

            this.onDestroy = obj => {
                onDestroy?.Invoke(obj);
                Object.Destroy(obj.gameObject);
            };
        }
    }
}
