using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(KillBullet());
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * 20 * Time.deltaTime);
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
        yield return new WaitForSeconds(8);
        gameObject.SetActive(false);
    }
}
