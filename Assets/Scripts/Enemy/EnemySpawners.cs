using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class EnemySpawners : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public int colourNum;
        public Color colour;
    }
    float distFromPlayer;
    float minDistForSpawn = 50;
    GameObject player;

    [Header("Pooling Reqs")]
    [SerializeField] ObjectPooler poolScript;
    [SerializeField] string poolTag;

    [Header("Enemy Types")]
    public List<EnemyType> enemyTypes;
    float timeBetweenSpawns = 2f;
    float timer = 2;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (RoundTimer.i.gameEnded) gameObject.SetActive(false);
        if (!RoundTimer.i.gamePlaying) return;

        // only activate spawning if this specific spawner is close enough to the player
        distFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distFromPlayer <= minDistForSpawn)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenSpawns)
            {
                RemoveCompletedColours();
                timer = 0;
                int num = Random.Range(0, enemyTypes.Count);
                SpawnEnemy(num);
            }
        }
    }

    void SpawnEnemy(int num)
    {
        GameObject newEnemy = poolScript.SpawnFromPoolIfInnactive(poolTag, transform.position, transform.rotation);
        if (newEnemy == null) return;
        Material eM = newEnemy.GetComponent<MeshRenderer>().material;
        eM.color = enemyTypes[num].colour;
        TMP_Text text = newEnemy.GetComponentInChildren<TMP_Text>();
        text.text = enemyTypes[num].colourNum.ToString();
        EnemyPaint eP = newEnemy.GetComponent<EnemyPaint>();
        eP.colourNum = enemyTypes[num].colourNum;
    }

    void RemoveCompletedColours()
    {
        for (int i = 0; i < enemyTypes.Count; i++)
        {
            bool temp = FloorFillCheck.ffc.CheckIfComplete(enemyTypes[i].colourNum);
            if (temp) enemyTypes.RemoveAt(i); 
        }
    }
}
