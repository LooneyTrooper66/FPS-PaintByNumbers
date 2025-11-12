using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        agent.destination = player.transform.position;
        if (Vector3.Distance(transform.position, player.transform.position) >= 40) gameObject.SetActive(false);
    }
}
