using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Marty
{
    public class GameObjectPool : ObjectPool<GameObject>
    {
        protected Action<GameObject> onCreate = null;
        protected new Action<GameObject> onRecycle = null;
        protected new Action<GameObject> onGet = null;
        protected new Action<GameObject> onReturn = null;
        protected new Action<GameObject> onDestroy = null;

        public GameObjectPool(GameObject template, int maxPoolSize = 0, Transform parent = null) :
            base(template, null, maxPoolSize)
        {
            createFunc = () => {
                GameObject obj = Object.Instantiate(template, parent);
                onCreate?.Invoke(obj);
                obj.SetActive(false);
                return obj;
            };

            base.onRecycle = obj => onRecycle?.Invoke(obj);

            base.onGet = obj => {
                onGet?.Invoke(obj);
                obj.SetActive(true);
            };

            base.onReturn = obj => {
                onReturn?.Invoke(obj);
                obj.SetActive(false);
            };

            base.onDestroy = obj => {
                onDestroy?.Invoke(obj);
                Object.Destroy(obj);
            };
        }

        public GameObjectPool OnCreate(Action<GameObject> onCreate)
        {
            this.onCreate = onCreate;
            return this;
        }

        public GameObjectPool OnRecycle(Action<GameObject> onRecycle)
        {
            this.onRecycle = onRecycle;
            return this;
        }

        public GameObjectPool OnGet(Action<GameObject> onGet)
        {
            this.onGet = onGet;
            return this;
        }

        public GameObjectPool OnReturn(Action<GameObject> onReturn)
        {
            this.onReturn = onReturn;
            return this;
        }

        public GameObjectPool OnDestroy(Action<GameObject> onDestroy)
        {
            this.onDestroy = onDestroy;
            return this;
        }
    }
}
