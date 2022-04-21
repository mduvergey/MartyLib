using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Marty
{
    public class ComponentPool<T> : GameObjectPool where T : Component
    {
        public ComponentPool(T template, int maxPoolSize = 0, Transform parent = null) :
            base(template.gameObject, maxPoolSize, parent)
        {
        }

        public ComponentPool<T> OnCreate(Action<T> onCreate)
        {
            if (onCreate != null)
            {
                this.onCreate = obj => onCreate.Invoke(obj.GetComponent<T>());
            }
            else
            {
                this.onCreate = null;
            }
            return this;
        }

        public ComponentPool<T> OnRecycle(Action<T> onRecycle)
        {
            if (onRecycle != null)
            {
                this.onRecycle = obj => onRecycle.Invoke(obj.GetComponent<T>());
            }
            else
            {
                this.onRecycle = null;
            }
            return this;
        }

        public ComponentPool<T> OnGet(Action<T> onGet)
        {
            if (onGet != null)
            {
                this.onGet = obj => onGet.Invoke(obj.GetComponent<T>());
            }
            else
            {
                this.onGet = null;
            }
            return this;
        }

        public ComponentPool<T> OnReturn(Action<T> onReturn)
        {
            if (onReturn != null)
            {
                this.onReturn = obj => onReturn.Invoke(obj.GetComponent<T>());
            }
            else
            {
                this.onReturn = null;
            }
            return this;
        }

        public ComponentPool<T> OnDestroy(Action<T> onDestroy)
        {
            if (onDestroy != null)
            {
                this.onDestroy = obj => onDestroy.Invoke(obj.GetComponent<T>());
            }
            else
            {
                this.onDestroy = null;
            }
            return this;
        }
    }
}
