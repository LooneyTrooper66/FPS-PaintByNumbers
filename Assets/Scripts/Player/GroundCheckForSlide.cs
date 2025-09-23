using System.Collections;
using UnityEngine;

public class GroundCheckForSlide : MonoBehaviour
{
    [SerializeField] PlayerControlls controls;
    Collider col;


    private void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            StartCoroutine(LandedTimer());
            col.enabled = false;
        }
            
    }

    IEnumerator LandedTimer()
    {
        controls.justLanded = true;
        yield return new WaitForSeconds(1);
        controls.justLanded = false;
    }
}
