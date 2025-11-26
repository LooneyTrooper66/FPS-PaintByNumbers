using System.Collections.Generic;
using UnityEngine;

public class FloorFillCheck : MonoBehaviour
{
    public static FloorFillCheck ffc;

    [System.Serializable]
    public class Colours
    {
        public int colourNum;
        public int numOfTilesOfColour;
        public int numOfTilesComplete;
        public bool colourComplete;
    }
    public List<Colours> colours;
    GameObject[] enemies;
    public int floorTilesCompleted = 0;


    private void Awake()
    {
        if (ffc != null) Destroy(this);
        else ffc = this;
    }
    private void Start() => enemies = GameObject.FindGameObjectsWithTag("Enemy");

    public void AddToTile(int numOfColour)
    {
        floorTilesCompleted++;
        int dictNum = numOfColour - 1;
        colours[dictNum].numOfTilesComplete++;

        if (colours[dictNum].numOfTilesComplete >= colours[dictNum].numOfTilesOfColour)
        {
            colours[dictNum].colourComplete = true;
            foreach(GameObject enemy in enemies)
            {
                EnemyPaint ep = enemy.GetComponent<EnemyPaint>();
                if (ep.colourNum == colours[dictNum].colourNum) enemy.SetActive(false);
            }
        }
    }

    public bool CheckIfComplete(int colourNum)
    {
        return colours[colourNum - 1].colourComplete;
    }
}
