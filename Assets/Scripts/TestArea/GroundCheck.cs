using UnityEngine;
using TMPro;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] TMP_Text roomNum;
    [SerializeField] LayerMask mask;


    private void FixedUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.up, out hit, 30, mask))
        {
            GroundNum num = hit.collider.gameObject.GetComponent<GroundNum>();
            if(num != null)
            {
                if (num.colourNum == 0) roomNum.text = "-";
                else roomNum.text = num.colourNum.ToString();
            }
            else roomNum.text = "-";
        }
    }
}
