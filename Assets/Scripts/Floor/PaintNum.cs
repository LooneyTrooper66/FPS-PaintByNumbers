using UnityEngine;

public class PaintNum : MonoBehaviour
{
    public int paintNum;
    Vector3 raycastStartPoint;
    LayerMask mask;


    private void Awake()
    {
        raycastStartPoint = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.2f, transform.localPosition.z);
        mask = LayerMask.GetMask("Floor");
    }

    // check the num of the plane its intersecting with, then return to the master script
    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastStartPoint, -Vector3.up, out hit, 5, mask))
        {
            GroundNum num = hit.collider.gameObject.GetComponent<GroundNum>();

            if (num.colourNum == 0) return;
            PercentCorrectCounter.pcc.numPaint++;
            if (num.colourNum == paintNum)
            {
                PercentCorrectCounter.pcc.numCorrect++;
                num.PaintFloorStep();
                PercentCorrectCounter.pcc.EnemyShot(true);
            }
            else PercentCorrectCounter.pcc.EnemyShot(false);
        }
    }
}
