using UnityEngine;

public class EnemyPaint : MonoBehaviour
{
    float raycastLength = 3;
    [SerializeField] GameObject paintDecal;
    ObjectPooler poolScript;
    [SerializeField] string poolTag;
    public int colourNum = 1;
    Material thisMat;
    MeshRenderer mR;
    GameObject player;
    float explodeTimer = 0;
    MeshRenderer explodeShell;
    Color halfWhite = new Vector4(1, 1, 1, 0.5f);
    float timeBeforeExplode = 3.5f;
    float distToStartTimer = 8;    
    AudioSource enemySource;
    public AudioClip explode;
    public AudioClip[] fizzs;
    bool fizzPlayed = false;


    private void Start()
    {
        thisMat = GetComponent<MeshRenderer>().material;
        GameObject poolScriptObj = GameObject.FindWithTag("GameController");
        poolScript = poolScriptObj.GetComponent<ObjectPooler>();
        player = GameObject.FindWithTag("Player");
        Transform child = gameObject.transform.GetChild(0);
        explodeShell = child.GetComponent<MeshRenderer>();
        enemySource = GetComponent<AudioSource>();
    }

    private void OnEnable() 
    { 
        explodeTimer = 0;
        fizzPlayed = false;
    }

    private void FixedUpdate()
    {
        // timer for when player gets within distance
        if (explodeTimer >= 0)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= distToStartTimer) explodeTimer += Time.deltaTime;
            else explodeTimer -= Time.deltaTime * 2;
        }
        else explodeTimer = 0;

        // lerp colour, speed of lerp scaled by the timer
        if (explodeTimer > 0)
        {
            float lerp = Mathf.PingPong(explodeTimer * explodeTimer * 1.2f, 1);
            explodeShell.material.color = Color.Lerp(Color.clear, halfWhite, lerp);
        }
        else explodeShell.material.color = Color.clear;

        // explode fizz sfx
        if (explodeShell.material.color.a >= 0.4 && !fizzPlayed)
        {
            enemySource.pitch = Random.Range(0.8f, 1f);
            enemySource.PlayOneShot(fizzs[Random.Range(0, 2)]);
            fizzPlayed = true;
        }
        else if (explodeShell.material.color.a <= 0.2 && fizzPlayed) fizzPlayed = false;

        if (explodeTimer >= timeBeforeExplode) Explode(1.5f); // scale of paint bigger if explode instead of shot
    }

    public void Explode(float scaleMultiplier)
    {
        PlacePaintDecal(scaleMultiplier);
        TriggerParticles();
        gameObject.SetActive(false);
    }

    void TriggerParticles()
    {
        GameObject pS = poolScript.SpawnFromPool(poolTag, transform.position, transform.rotation);
        ParticleSystem ps = pS.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = thisMat.color;
        AudioSource particleSource = pS.GetComponent<AudioSource>();
        particleSource.pitch = Random.Range(0.8f, 1.1f);
        particleSource.PlayOneShot(explode);
        ps.Play();
    }

    void PlacePaintDecal(float scaleMultiplier)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.TransformDirection(Vector3.up), out hit, raycastLength);
        if (hit.collider.gameObject.tag == "Surface")
        {
            Vector3 spawnPaint = new Vector3(transform.position.x, hit.collider.gameObject.transform.position.y + 0.001f, transform.position.z); // add 0.01 to y so stacks ontop without clipping
            GameObject thisPaint = Instantiate(paintDecal, spawnPaint, transform.rotation); // i know this is unpotomised but id rather do this then make a pool of like 10,000 lol + this is a demo so womp womp
            thisPaint.transform.localScale = thisPaint.transform.localScale * scaleMultiplier;
            PaintNum pN = thisPaint.GetComponent<PaintNum>();
            pN.paintNum = colourNum;
            mR = thisPaint.GetComponent<MeshRenderer>();
            mR.material.color = thisMat.color;
        }
    }
}
