using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject target;
    public Vector3 targetPos;
    public string targetTag;


    private void OnEnable()
    {
        StartCoroutine(KillBullet());
        targetPos = Vector3.zero;
    }

    public void SetTarget(GameObject gO, Vector3 pos, string tag)
    {
        target = gO;
        targetPos = pos;
        targetTag = tag;
    }

    private void Update()
    {
        if (targetPos != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 80 * Time.deltaTime);
            if (target.activeSelf && Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                if (targetTag == "Enemy") target.GetComponent<EnemyPaint>().Explode(1);
                gameObject.SetActive(false);
            }
        }
        else transform.Translate(Vector3.forward * 80 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Surface")
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator KillBullet()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
