using UnityEngine;

public class EnemyPaint : MonoBehaviour
{
    float raycastLength = 3;
    [SerializeField] GameObject paintDecal;
    ObjectPooler poolScript;
    [SerializeField] string poolTag;
    [HideInInspector] public int colourNum = 1;
    Material thisMat;
    MeshRenderer mR;
    GameObject player;
    float explodeTimer = 0;
    MeshRenderer explodeShell;
    Color notExplode = new Color(255, 255, 255, 0);
    Color yesExplode = new Color(255, 255, 255, 225);


    private void Start()
    {
        thisMat = GetComponent<MeshRenderer>().material;
        GameObject poolScriptObj = GameObject.FindWithTag("GameController");
        poolScript = poolScriptObj.GetComponent<ObjectPooler>();
        player = GameObject.FindWithTag("Player");
        explodeShell = GetComponentInChildren<MeshRenderer>();
    }

    private void OnEnable() { explodeTimer = 0; explodeShell.enabled = false; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            collision.gameObject.SetActive(false);
            PlacePaintDecal();
            TriggerParticles();
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= 5) explodeTimer += Time.deltaTime;
        else if (Vector3.Distance(transform.position, player.transform.position) > 5 && explodeTimer <= 0) explodeTimer -= Time.deltaTime;

        if (explodeTimer >= 0)
        {
            float lerp = Mathf.PingPong(Time.deltaTime, 1) / explodeTimer;
            explodeShell.material.color = Color.Lerp(notExplode, yesExplode, lerp);
        }
        else
        {
            explodeShell.enabled = false;
            explodeShell.material.color = notExplode;
        }

        if (explodeTimer >= 5) Explode();
    }

    void Explode()
    {
        PlacePaintDecal();
        TriggerParticles();
        gameObject.SetActive(false);
    }

    void TriggerParticles()
    {
        GameObject pS = poolScript.SpawnFromPool(poolTag, transform.position, transform.rotation);
        ParticleSystem ps = pS.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = thisMat.color;
        ps.Play();
    }

    void PlacePaintDecal()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.TransformDirection(Vector3.up), out hit, raycastLength);
        if (hit.collider.gameObject.tag == "Surface")
        {
            Vector3 spawnPaint = new Vector3(transform.position.x, hit.collider.gameObject.transform.position.y + 0.001f, transform.position.z); // add 0.01 to y so stacks ontop without clipping
            GameObject thisPaint = Instantiate(paintDecal, spawnPaint, transform.rotation); // i know this is unpotomised but id rather do this then make a pool of like 10,000 lol + this is a demo so womp womp
            PaintNum pN = thisPaint.GetComponent<PaintNum>();
            pN.paintNum = colourNum;
            mR = thisPaint.GetComponent<MeshRenderer>();
            mR.material.color = thisMat.color;
        }
    }
}
