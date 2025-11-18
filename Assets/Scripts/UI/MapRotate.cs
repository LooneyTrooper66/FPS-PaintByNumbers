using UnityEngine;

public class MapRotate : MonoBehaviour
{
    public Vector3 rotateAngel;


    private void Update()
    {
        transform.Rotate(rotateAngel * Time.deltaTime);
    }
}
