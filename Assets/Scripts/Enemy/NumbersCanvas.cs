using UnityEngine;

public class NumbersCanvas : MonoBehaviour
{
    Transform camTransform;


    private void Start()
    {
        camTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + camTransform.rotation * -Vector3.forward, camTransform.rotation * Vector3.up);
    }
}
