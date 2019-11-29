/*
Nikola

Created: Nov 28/19
Last Modified: Nov 28/19


This hook mechanic constantly calculates the distance between the player and the object it's hooking on to
it will keep applying force, until the distance starts to become greater

 
  */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMechanic : MonoBehaviour
{
    [SerializeField]
    private float hookRadius; //collider radius
    private SphereCollider hookSphereCollider; //collider component

    private bool pointInRange; //is the target in range?
    private float distToHookPoint; // distance to hookpoint in sphere collider range

    [SerializeField]
    private float forceAmount; // force amount for push (approx 130 is at a point, where it still sends player forward)

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
        if (other.gameObject.name.Contains("HookPoint"))
        {
            nearestHookPoint = other;
            distToHookPoint = Vector3.Distance(other.transform.position, this.transform.position);
            Debug.Log(distToHookPoint);
            //rb.AddForce((other.transform.position - this.transform.position).normalized * forceAmount * Time.smoothDeltaTime); 
        }

        
    }
        void OnTriggerExit(Collider other)
    {
        nearestHookPoint = null;
        shortestDistance = new Vector3(15, 15, 15); 
         Debug.Log("trigger exit"); 
    }
    // Update is called once per frame
    void Update()
    {
        if (nearestHookPoint != null)
        {
            Vector3 temp = nearestHookPoint.transform.position - this.transform.position; 

            if (shortestDistance.magnitude > temp.magnitude)
            {
                shortestDistance = temp;
            }else
            {
                nearestHookPoint = null;
                
            }
            //Debug.Log("trying to push!!" + temp);
            rb.AddForce((temp * forceAmount * Time.smoothDeltaTime)); 

        }
    }
}
