using UnityEngine;

public class LookCamera : MonoBehaviour
{
    private void Start()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
