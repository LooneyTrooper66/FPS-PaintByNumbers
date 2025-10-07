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

    [Header("Pooling Reqs")]
    [SerializeField] ObjectPooler poolScript;
    [SerializeField] string poolTag;

    [Header("Enemy Types")]
    public List<EnemyType> enemyTypes;
    float timeBetweenSpawns = 1.5f;
    float timer = 0;


    private void FixedUpdate()
    {
        // spawn enemies in intervals of a few seconds, stop when no more left innactive in pool and start again as soon as a new one becomes active
        //if (poolScript has innactive gameobjects in it)
        timer += Time.deltaTime;

        if (timer >= timeBetweenSpawns)
        {
            timer = 0;
            int num = Random.Range(0, enemyTypes.Count);
            SpawnEnemy(num);
        }
    }

    void SpawnEnemy(int num)
    {
        GameObject newEnemy = poolScript.SpawnFromPool(poolTag, transform.position, transform.rotation);
        Material eM = newEnemy.GetComponent<MeshRenderer>().material;
        eM.color = enemyTypes[num].colour;
        TMP_Text text = newEnemy.GetComponentInChildren<TMP_Text>();
        text.text = enemyTypes[num].colourNum.ToString();
    }
}
