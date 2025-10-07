using UnityEngine;

public class EnemyPaint : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {


            gameObject.SetActive(false);
        }
    }
}
