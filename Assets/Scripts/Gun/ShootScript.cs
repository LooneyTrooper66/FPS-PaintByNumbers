using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public ObjectPooler poolScript;
    public string poolTag;
    public Transform shootPoint;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            poolScript.SpawnFromPool(poolTag, shootPoint.position, shootPoint.rotation);
        }
    }
}
