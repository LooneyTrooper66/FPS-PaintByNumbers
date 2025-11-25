using UnityEngine;

public class ExplodeAnim : MonoBehaviour
{
    GameObject player;
    float explodeTimer = 0;
    MeshRenderer explodeShell;
    Color halfWhite = new Vector4(1, 1, 1, 0.3f);


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        Transform child = gameObject.transform.GetChild(0);
        explodeShell = child.GetComponent<MeshRenderer>();    
    }

    private void OnEnable()
    {
        explodeTimer = 0;
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
            if (Vector3.Distance(transform.position, player.transform.position) <= 8) explodeTimer += Time.deltaTime;
            else explodeTimer -= Time.deltaTime * 2;
        }
        else explodeTimer = 0;

        if (explodeTimer > 0)
        {
            float lerp = Mathf.PingPong(explodeTimer * explodeTimer, 1);
            explodeShell.material.color = Color.Lerp(Color.clear, halfWhite, lerp);
        }
        else
        {
            explodeShell.material.color = Color.clear;
        }

        if (explodeTimer >= 5) Debug.Log("Explode");
    }
}
