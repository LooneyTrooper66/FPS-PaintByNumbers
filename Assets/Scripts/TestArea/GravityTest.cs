using UnityEngine;
using UnityEngine.EventSystems;

public class GravityTest : MonoBehaviour
{
    public float StrengthOfAttraction = 10;
    public float snapDist = 0.2f;
    GameObject planet;
    Rigidbody rb;

    bool attracting;
    Vector3 offset;


    void Start()
    {
        planet = GameObject.FindGameObjectWithTag("Finish");
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            attracting = !attracting;
        }
    }

    void FixedUpdate()
    {
        if (attracting)
        {
            offset = planet.transform.position - transform.position;

            if (Vector3.Distance(transform.position, planet.transform.position) < snapDist) transform.position = planet.transform.position;
            else rb.linearVelocity = new Vector3((StrengthOfAttraction * offset.normalized.x) * rb.mass, (StrengthOfAttraction * offset.normalized.y) * rb.mass, (StrengthOfAttraction * offset.normalized.z) * rb.mass);
        }
        else { rb.AddForce(Vector3.up * Physics.gravity.y, ForceMode.Force); }
    }
}

    /*Transform gravCentre;
    ConstantForce grav;


    private void Start()
    {
        gravCentre = GameObject.FindGameObjectWithTag("Finish").transform;
        grav = GetComponent<ConstantForce>();
    }

    private void Update()
    {
        float gravDist = Vector3.Distance(gravCentre.position, transform.position);
        grav.force = gravCentre.position/gravDist;
    }*/
