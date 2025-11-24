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
                if (num.colourNum == 0) return; /*{ roomNum.text = "Current Room: -"; roomNum.color = Color.white; }*/
                else { roomNum.text = $"Current Room: {num.colourNum}"; roomNum.color = num.paintCol; }
            }
            else roomNum.text = "Current Room: -";
        }
    }
}
