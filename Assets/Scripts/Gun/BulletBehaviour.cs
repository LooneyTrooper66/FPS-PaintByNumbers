using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{
    public Vector3 targetPos;
    public string targetTag;


    private void OnEnable()
    {
        StartCoroutine(KillBullet());
        targetPos = Vector3.zero;
    }

    public void SetTarget(Vector3 pos, string tag)
    {
        targetPos = pos;
        targetTag = tag;
    }

    private void Update()
    {
        if (targetPos != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 80 * Time.deltaTime);
            if (targetTag != null && Vector3.Distance(transform.position, targetPos) < 0.05f) gameObject.SetActive(false);
        }
        else transform.Translate(Vector3.forward * 80 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Surface")
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
