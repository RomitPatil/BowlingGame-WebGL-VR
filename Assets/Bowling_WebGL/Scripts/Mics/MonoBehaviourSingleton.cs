using UnityEngine;

namespace meetme.mics
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _singleton;
        public static T singleton
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = FindObjectOfType<T>();
                    if (_singleton == null)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        _singleton = go.AddComponent<T>();
                    }
                }
                return _singleton;
            }
        }

        protected void CreateSingleton(bool dontDestroyOnLoad = true)
        {
            if (_singleton == null)
            {
                _singleton = this as T;
                if (dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }
            else if (_singleton != this)
            {
                Destroy(gameObject);
            }
        }
    }
}