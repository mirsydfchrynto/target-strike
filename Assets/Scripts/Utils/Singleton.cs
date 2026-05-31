using UnityEngine;

namespace TargetStrike.Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();

        public static bool HasInstance => _instance != null;

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindAnyObjectByType(typeof(T));

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = typeof(T).ToString() + " (Singleton)";
                            if (Application.isPlaying)
                                DontDestroyOnLoad(singleton);
                        }
                    }
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                if (Application.isPlaying)
                    DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
