using UnityEngine;

public class ThomasGameThing : MonoBehaviour
{

    public Transform floatPoint;
    public float launchSpeed;

    Camera cam;

    GameObject target;
    Rigidbody targetRig;
    public float StrengthOfAttraction = 10;
    public float snapDist = 0.1f;

    public float weaponRange = 10f;
    // make sure the raycast wont hit the player
    public LayerMask mask; 

    bool isAttracting;
    bool isLaunching;


    void Start() => cam = Camera.main;

    void Update()
    {
        //Attract Input, Attract with LMB
        if (Input.GetButtonDown("Fire1"))
        {
            isAttracting = !isAttracting; isLaunching = false; 
        }

        //Throw With RMB
        if (isAttracting)
        {
            if (Input.GetButtonDown("Fire2") && target != null) Throw(); 
        }
    }

    private void FixedUpdate()
    {
        if (isAttracting)
            Attract();
        else if (!isAttracting && target != null)
            Release();
    }

    //Attraction Code
    private void Attract()
    {
        if (isLaunching) return;

        if (target == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weaponRange, mask))
            {
                if (hit.transform.tag == "Magnetised")
                {
                    target = hit.transform.gameObject;
                    targetRig = target.GetComponent<Rigidbody>();
                    targetRig.useGravity = false;
                    target.transform.position = Vector3.MoveTowards(target.transform.position, floatPoint.position, 0.3f);
                }
                else isAttracting = false;
            }
            else isAttracting = false;
        }
        else
        {
            Vector3 offset = floatPoint.transform.position - target.transform.position;

            if (Vector3.Distance(target.transform.position, floatPoint.transform.position) < snapDist) targetRig.linearVelocity = Vector3.zero; //target.transform.position = floatPoint.transform.position;
            else targetRig.linearVelocity = new Vector3((StrengthOfAttraction * offset.normalized.x) * targetRig.mass, (StrengthOfAttraction * offset.normalized.y) * targetRig.mass, (StrengthOfAttraction * offset.normalized.z) * targetRig.mass);
        }
    }

    //Release Code
    private void Release()
    {
        targetRig.linearVelocity = Vector3.zero;
        targetRig.useGravity = true;
        target = null;
    }

    //Throw Code
    private void Throw()
    {
        if (target != null)
        {
            targetRig.useGravity = true;
            targetRig.linearVelocity = Vector3.zero;
            targetRig.AddForce(floatPoint.transform.forward * launchSpeed, ForceMode.Impulse);
            target = null;
            isLaunching = false; isAttracting = false;
        }
        else { isLaunching = false; isAttracting = true; }
    }
}
