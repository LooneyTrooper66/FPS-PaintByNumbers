using UnityEngine;
using UnityEngine.InputSystem;

public class UpdatedPlayerLook : MonoBehaviour
{
    [SerializeField] float mouseSense = 1f;
    [SerializeField] Transform body;
    [SerializeField] InputActionAsset map;
    InputAction mouseLookAction;
    Vector2 mouseLook;
    float mouseX;
    public RoundTimer timer;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mouseLookAction = map.FindAction("Look");
    }

    private void Update()
    {
        if (timer.gamePlaying)
        {
            mouseLook = mouseLookAction.ReadValue<Vector2>();
            mouseX -= mouseLook.y * mouseSense * Time.deltaTime;
            mouseX = Mathf.Clamp(mouseX, -90f, 90f);

            transform.localRotation = Quaternion.Euler(mouseX, 0, 0);
            body.Rotate(Vector3.up * mouseX);
        }
    }
}
