using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Marty
{
    public class ComponentPool<T> : ObjectPool<T> where T : Component
    {
        public ComponentPool(T template, int maxPoolSize = 0,
            Action<T> onCreate = null,
            Action<T> onRecycle = null,
            Action<T> onGet = null,
            Action<T> onReturn = null,
            Action<T> onDestroy = null,
            Transform parent = null) :
            base(template, null, maxPoolSize, onRecycle)
        {
            this.createFunc = () => {
                T obj = Object.Instantiate(template, parent);
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
