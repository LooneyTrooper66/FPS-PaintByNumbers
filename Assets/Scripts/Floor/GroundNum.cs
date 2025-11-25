using UnityEngine;

public class GroundNum : MonoBehaviour
{
    public int colourNum;
    public Color paintCol;
    MeshRenderer mr;
    public float numOfKillsToFullColour;
    float step = 0;
    float percentColoured = 0;


    private void Start() => mr = GetComponent<MeshRenderer>();

    public void PaintFloorStep()
    {
        step++;
        percentColoured = step / numOfKillsToFullColour;

        mr.material.color = Color.Lerp(Color.white, paintCol, percentColoured);

        if (step == numOfKillsToFullColour) FloorFillCheck.ffc.AddToTile(colourNum);
    }
}
