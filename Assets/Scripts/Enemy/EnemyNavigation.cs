using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    float timer;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 60) gameObject.SetActive(false); // if been alive more than 60 seconds deactivate
    }

    private void FixedUpdate()
    {
        agent.destination = player.transform.position;
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= 3) agent.destination = transform.position;
        else if (dist >= 50) gameObject.SetActive(false);
    }
}
