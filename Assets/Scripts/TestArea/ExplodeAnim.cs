using UnityEngine;

public class ExplodeAnim : MonoBehaviour
{
    GameObject player;
    float explodeTimer = 0;
    Transform child;
    MeshRenderer explodeShell;
    Color notExplode = new Color(255, 255, 255, 0);
    Color yesExplode = new Color(255, 255, 255, 225);


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        child = transform.GetChild(0);
        explodeShell = child.GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        explodeTimer = 0;
        if (explodeShell == null) Debug.Log("not exists");
        //explodeShell.enabled = false; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            collision.gameObject.SetActive(false);
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

        if (explodeTimer >= 5) Debug.Log("Explode");
    }
}
