using UnityEngine;

namespace Marty
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; } = null;

        private void Awake()
        {
            if ((Instance != null) && (Instance != this))
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = (T)this;
                DontDestroyOnLoad(gameObject);
                OnAwake();
            }
        }

        protected virtual void OnAwake() { }
    }
}
