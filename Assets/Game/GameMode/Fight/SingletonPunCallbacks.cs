using Photon.Pun;
using UnityEngine;

public class SingletonPunCallbacks<T> : MonoBehaviourPunCallbacks where T: SingletonPunCallbacks<T>
{
    private static T instance;
    public static T I
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null) {
                    GameObject _gameObject = new GameObject(typeof(T).Name);
                    instance = _gameObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public virtual void Awake() {
        if(instance != null) Destroy(gameObject);
    }
}