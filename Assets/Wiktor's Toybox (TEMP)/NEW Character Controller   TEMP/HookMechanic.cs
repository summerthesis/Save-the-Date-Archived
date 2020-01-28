/*
Nikola

Created: Nov 28/19
Last Modified: Jan 23/2020

When the player presses the space bar, they will have a force pushing them to just below the "hook object" towards the "hook point"
when the player collides with the "hook point" they will stop their x/y/z velocity, and fall to the ground below (due to gravity)

 
  */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMechanic : MonoBehaviour
{
    [SerializeField]
    private float hookRadius; 
    //collider radius
    private SphereCollider hookSphereCollider; 
    //collider component

    private bool pointInRange; 
    //is the target in range?
    private float distToHookPoint;
    // distance to hookpoint in sphere collider range
    private bool isHookEnabled = false;
    //only hook after button has been pressed

    [SerializeField]
    private float forceAmount;
    // force amount for push (approx 130 is at a point, where it still sends player forward)


    [SerializeField]
    private bool gravityDuringPull = false; 
    //variable to determine if graivty should be enabled during hook(pull). 
    // false will disable gravity. true will keep gravity enabled

    private Rigidbody rb; //player's rigid body component (to add force to, later on)
    private Collider nearestHookPoint;  //collider aspect of nearest point (for distance calculation)
    private Vector3 shortestDistance = new Vector3(20,20,20); // minimum distance checker

    // Start is called before the first frame update
    void Start()
    {
        hookSphereCollider = this.gameObject.GetComponent<SphereCollider>();
        hookSphereCollider.radius = hookRadius;
        rb = GetComponentInParent<Rigidbody>(); 

    }
    void OnTriggerEnter(Collider other)
    {
        // checks to see if HookPoint is within range
        if (other.gameObject.name.Contains("HookObject")) 
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
        }

    }
        void OnTriggerExit(Collider other)
    {
        nearestHookPoint = null;
        shortestDistance = new Vector3(hookRadius, hookRadius, hookRadius); 
         Debug.Log("trigger exit"); 
    }
    // Update is called once per frame
    void Update()
    {
        //checks to see if player has pressed hook button and if hook is within range
        if (nearestHookPoint != null && isHookEnabled) 
        {
            Vector3 temp = nearestHookPoint.transform.position - this.transform.position; 
             Debug.DrawLine(this.transform.position, nearestHookPoint.transform.position, Color.red, 5.0f, true); 
            if (shortestDistance.magnitude > temp.magnitude && shortestDistance.magnitude > 1) 
            {
                /*
                if (gravityDuringPull == false)
                {
                    rb.useGravity = false; 
                }*/
                shortestDistance = temp; 
                rb.AddForce((temp * forceAmount * Time.smoothDeltaTime)); 
            }
            else
            {
                rb.useGravity = true; 
                isHookEnabled = false;
                shortestDistance = new Vector3(hookRadius, hookRadius, hookRadius); 
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            
            if (nearestHookPoint != null)
            {
                Debug.DrawLine(this.transform.position, nearestHookPoint.transform.position, Color.blue, 5.0f, true);
                isHookEnabled = true; 
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //rb.useGravity = true;
            //nearestHookPoint = null;
            isHookEnabled = false;
            shortestDistance = new Vector3(hookRadius, hookRadius, hookRadius); 

        }
    }
}
