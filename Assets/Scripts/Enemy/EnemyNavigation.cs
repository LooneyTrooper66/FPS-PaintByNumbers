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
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= 3) agent.destination = transform.position;
        else if (dist >= 70) gameObject.SetActive(false);
    }
}
