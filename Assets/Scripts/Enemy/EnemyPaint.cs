using UnityEngine;

public class EnemyPaint : MonoBehaviour
{
    [SerializeField] float castRadius = 3;
    [SerializeField] GameObject paint;
    [SerializeField] ObjectPooler poolScript;
    [SerializeField] string poolTag;
    Material thisMat;
    MeshRenderer mR;


    private void Start()
    {
        thisMat = GetComponent<MeshRenderer>().material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            PaintExplode();
            TriggerParticles();
            gameObject.SetActive(false);
        }
    }

    void TriggerParticles()
    {
        GameObject pS = poolScript.SpawnFromPool(poolTag, transform.position, transform.rotation);
        ParticleSystem ps = pS.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = thisMat.color;
        ps.Play();
    }

    void PaintExplode()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.TransformDirection(Vector3.up), out hit, castRadius);
        if (hit.collider.gameObject.tag == "Surface")
        {
            Vector3 spawnPaint = new Vector3(transform.position.x, hit.collider.gameObject.transform.position.y + 0.01f, transform.position.z); // add 0.01 to y so stacks ontop without clipping
            GameObject thisPaint = Instantiate(paint, spawnPaint, transform.rotation);
            mR = thisPaint.GetComponent<MeshRenderer>();
            mR.material.color = thisMat.color;
        }
    }
}
