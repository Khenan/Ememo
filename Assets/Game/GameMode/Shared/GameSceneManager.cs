using UnityEngine;

public class GameSceneManager : Singleton<GameSceneManager>
{
    [SerializeField] private Transform root;
    public override void Awake() {
        base.Awake();
        root.gameObject.SetActive(false);
    }
    public virtual void StartScene()
    {
        root.gameObject.SetActive(true);
    }
    public void StopScene()
    {
        root.gameObject.SetActive(false);
    }
}