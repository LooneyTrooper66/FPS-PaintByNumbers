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

    private void OnEnable()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 80) gameObject.SetActive(false); // if been alive more than 60 seconds deactivate

        if (RoundTimer.i.gameEnded) gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        agent.destination = player.transform.position;
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= 4) agent.destination = transform.position;
        else if (dist >= 90) gameObject.SetActive(false);
    }
}
