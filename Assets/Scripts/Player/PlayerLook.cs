using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float mouseSense = 1f;
    [SerializeField] Transform body;
    float xRotation;
    public RoundTimer timer;


    private void LateUpdate()
    {
        // lock x movement/rotation while sliding

        if(timer.gamePlaying)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSense;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSense;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            body.Rotate(Vector3.up * mouseX);
        }
    }
}
