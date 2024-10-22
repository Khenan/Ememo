using UnityEngine;
public class Singleton : MonoBehaviour {

}
public class Singleton<T> : Singleton where T: Singleton<T>
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
