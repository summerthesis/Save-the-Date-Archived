using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NewHookMechT : MonoBehaviour
{

    private bool pointInRange;
    //is the target in range?
    private float distToHookPoint;
    // distance to hookpoint in sphere collider range
    private bool isHookEnabled = false;
    //only hook after button has been pressed

    [SerializeField]
    private float forceAmount;
    // force amount for push (approx 130 is at a point, where it still sends player forward)
    private Rigidbody rb; //player's rigid body component (to add force to, later on)
    private Collider nearestHookPoint;  //collider aspect of nearest point (for distance calculation)


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("NewHookMech Script Started");
        rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (nearestHookPoint != null && isHookEnabled)
        {
            Vector3 temp = nearestHookPoint.transform.position - this.transform.position;
            Debug.DrawLine(this.transform.position, nearestHookPoint.transform.position, Color.red, 5.0f, true);

            rb.AddForce((temp * forceAmount * Time.smoothDeltaTime));
            /*
            if (shortestDistance.magnitude > temp.magnitude && shortestDistance.magnitude > 1)
            {
                shortestDistance = temp;
                rb.AddForce((temp * forceAmount * Time.smoothDeltaTime));
            }
            else
            {
                //rb.useGravity = true;
                isHookEnabled = false;
                //shortestDistance = new Vector3(hookRadius, hookRadius, hookRadius);
            }*/

        }


        if (Input.GetKeyDown(KeyCode.E))
        {

            if (nearestHookPoint != null)
            {
                Debug.DrawLine(this.transform.position, nearestHookPoint.transform.position, Color.blue, 5.0f, true);
                isHookEnabled = true;
            }
        }

    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("NewHookMech - collider entered: " + other);
        // checks to see if HookPoint is within range
        if (other.gameObject.name.Contains("HookType"))
        {

            //set the nearest hook point
            nearestHookPoint = other;
            // calculate the distance from the player to the hook point
            distToHookPoint = Vector3.Distance(other.transform.position, this.transform.position);
            Debug.Log("Distance: " + distToHookPoint);
            Debug.DrawLine(this.transform.position, nearestHookPoint.transform.position, Color.red, 5.0f, true);

        }
        Debug.Log("Transform Position:" + other.transform.position);

        if (other.gameObject.name.Contains("HookPoint"))
        {
            //stop all other velocity of player, so they fall down right below hook point
            this.rb.velocity = new Vector3(0, 0, 0);
            Debug.Log("Hook point hit!!");
            isHookEnabled = false;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("HookType"))
        {
            isHookEnabled = false;
            nearestHookPoint = null;
            Debug.Log("Hook point left...");
        }
    }

}
