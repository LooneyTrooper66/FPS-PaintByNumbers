using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    // this script is one i wrote for another project. i did follow a tutorial for it and i'll link the video if i can find it
    // https://youtu.be/tdSmKaJvCoA?si=L384BkY6m4FWALNL <-- tutorial i followed

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("pool with tag " + tag + " doesnt exist");
            return null;
        }

        GameObject objToSpawn = poolDictionary[tag].Dequeue();

        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;
        objToSpawn.SetActive(true);

        poolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }

    public GameObject SpawnFromPoolIfInnactive(string tag, Vector3 position, Quaternion rotation)
    {
        // checks if the dictionary i want to take from exists
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("pool with tag " + tag + " doesnt exist");
            return null;
        }

        // if gameobject is innactive, set it to nextAvailableGO and break for loop
        // it its active, requeue it and check the next one
        GameObject nextAvailableGO = null;
        for (int i = 0; i < poolDictionary[tag].Count;)
        {
            GameObject nextObj = poolDictionary[tag].Dequeue();

            if (nextObj.activeSelf == false)
            {
                nextAvailableGO = nextObj;
                break;
            }
            else
            {
                poolDictionary[tag].Enqueue(nextObj);
                i++;
                if (i > poolDictionary[tag].Count) i = 0;
                //Debug.Log(poolDictionary[tag].Count.ToString() + i.ToString());
            }
        }

        GameObject objToSpawn = nextAvailableGO;

        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;
        objToSpawn.SetActive(true);

        poolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }
}