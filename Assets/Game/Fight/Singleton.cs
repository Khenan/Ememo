using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null) instance = Instantiate(new GameObject()).AddComponent<T>();
            }
            return instance;
        }
    }

    private void Awake() {
        if(instance != null) Destroy(gameObject);
    }
}