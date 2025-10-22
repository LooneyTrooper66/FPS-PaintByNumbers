using UnityEngine;

public class ExplodeAnim : MonoBehaviour
{
    GameObject player;
    float explodeTimer = 0;
    MeshRenderer explodeShell;
    Color notExplode = new Color(255, 255, 255, 0);
    Color yesExplode = new Color(255, 255, 255, 1);


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        explodeShell = gameObject.GetComponentInChildren<MeshRenderer>();
    }

    private void OnEnable()
    {
        explodeTimer = 0;
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
        // timer for when player gets within distance
        if (explodeTimer >= 0)        
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= 5) explodeTimer += Time.deltaTime;
            else explodeTimer -= Time.deltaTime;
        }
        else explodeTimer = 0;

        //Debug.Log(explodeTimer);

        // set alpha of the mesh depending on timer
        if (explodeTimer > 0)
        {
            explodeShell.material.color = Color.white;
        }
        else
        {
            explodeShell.material.color = Color.clear;
        }
        // if (explodeTimer > 0)
        // {
        //     float lerp = Mathf.PingPong(Time.deltaTime, 1) / explodeTimer;
        //     explodeShell.material.color = Color.Lerp(notExplode, yesExplode, lerp);
        // }
        // else
        // {
        //     //explodeShell.enabled = false;
        //     explodeShell.material.color = notExplode;
        // }

        if (explodeTimer >= 5) Debug.Log("Explode");
    }
}
