using UnityEngine;

public class EnemyPaint : MonoBehaviour
{
    [SerializeField] float castRadius = 3;
    [SerializeField] GameObject paint;
    Material thisMat;
    MeshRenderer mR;
    ParticleSystem pS;


    private void Start()
    {
        thisMat = GetComponent<MeshRenderer>().material;
        pS = GetComponent<ParticleSystem>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            PaintExplode();
            // particles 
            gameObject.SetActive(false);
        }
    }

    void PaintExplode()
    {
        // spherecast and check how much of it intersected with anything tagged 'surface'
        // use that size to determine where will turn red

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
