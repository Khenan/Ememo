using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Transform root;
    private Transform target = null;
    void Update()
    {
        if(root != null && target != null) root.position = Vector3.Lerp(root.position, target.position, Time.deltaTime * 5);
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}