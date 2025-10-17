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
    public float minDistForSpawn = 50f;
    GameObject player;

    [Header("Pooling Reqs")]
    [SerializeField] ObjectPooler poolScript;
    [SerializeField] string poolTag;

    [Header("Enemy Types")]
    public List<EnemyType> enemyTypes;
    float timeBetweenSpawns = 2.5f;
    float timer = 0;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        // only activate spawning if this specific spawner is close enough to the player
        distFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        //Debug.Log(distFromPlayer);
        if (distFromPlayer <= minDistForSpawn)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenSpawns)
            {
                timer = 0;
                int num = Random.Range(0, enemyTypes.Count);
                SpawnEnemy(num);
            }
        }
    }

    void SpawnEnemy(int num)
    {
        GameObject newEnemy = poolScript.SpawnFromPoolIfInnactive(poolTag, transform.position, transform.rotation);
        Material eM = newEnemy.GetComponent<MeshRenderer>().material;
        eM.color = enemyTypes[num].colour;
        TMP_Text text = newEnemy.GetComponentInChildren<TMP_Text>();
        text.text = enemyTypes[num].colourNum.ToString();
        EnemyPaint eP = newEnemy.GetComponent<EnemyPaint>();
        eP.colourNum = num;
    }
}
