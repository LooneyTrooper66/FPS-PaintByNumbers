using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public ObjectPooler poolScript;
    public string poolTag;
    public Transform shootPoint;
    public LayerMask bulletMask;
    public AudioClip shootSound;
    public AudioSource gunSource;

    float shootTimer = 0;
    bool canShoot = true;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && RoundTimer.i.gamePlaying && canShoot)
        {
            canShoot = false;
            gunSource.PlayOneShot(shootSound);
            gunSource.pitch = Random.Range(0.8f, 1);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f, bulletMask))
            {
                GameObject bullet = poolScript.SpawnFromPool(poolTag, shootPoint.position, shootPoint.rotation);
                bullet.GetComponent<BulletBehaviour>().SetTarget(hit.collider.gameObject, hit.point, hit.collider.gameObject.tag);
            }
            else poolScript.SpawnFromPool(poolTag, shootPoint.position, shootPoint.rotation);
            shootTimer = 0;
        }

        shootTimer += Time.deltaTime;
        if (shootTimer >= 0.2) canShoot = true;
    }
}
