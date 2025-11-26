using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float mouseSense = 1f;
    [SerializeField] Transform body;
    [SerializeField] GameObject CamPosAfterGame;
    [SerializeField] GameObject fireflies;
    [SerializeField] GameObject gun;
    float xRotation;


    private void LateUpdate()
    {
        if(RoundTimer.i.gamePlaying)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSense;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSense;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            body.Rotate(Vector3.up * mouseX);
        }
        
        if (RoundTimer.i.gameEnded)
        {
            gun.SetActive(false);
            fireflies.SetActive(true);
            transform.parent = CamPosAfterGame.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}
