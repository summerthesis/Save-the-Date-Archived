/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * Charge Handler
 * Created: November 14, 2019
 * Last Modified: November 18, 2019
 * Last Used: November 28, 2019
 *  
 * DISCARDED CLASS!
 *      Reason: Target detection system changed
 *  
 * Inherits from MonoBehaviour
 * 
 * - Detects targets' proximity using collision system.
 *   Should come without saying that it requires a collider,
 *   currently set to SphereCollider.
 * 
 * - Allows other components/objects to know whether there's a 
 *   chargeable object in the vicinity.
 *   
 * - Also allows objects access to a reference to the Chargeable 
 *   component of the target, should it be necessary
 * 
 * - It is imperative that this remain in the specific "ThunderPhysics"
 *   collision layer, to avoid "false positive" collision events
 *   
 * - Question: What if two chargeable colliders are close to each other?
 * 
 ********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TargetTracker : MonoBehaviour
{
    [SerializeField] private float chargeRadius; //collider radius
    private SphereCollider sphereCollider; //collider component

    private bool targetInRange; //is the target in range?
    public bool TargetInRange { get { return targetInRange; } }

    private Chargeable chargeableTarget; //chargeable component (of target)
    public Chargeable Charger { get { return chargeableTarget ; } }

    /// <summary>
    /// Script init functions. Gets references to the components and targets
    /// </summary>
    void Awake()
    {
        targetInRange = false;
        sphereCollider = this.gameObject.GetComponent<SphereCollider>();
        sphereCollider.radius = chargeRadius;
        chargeableTarget = null;
    }
    
    /// <summary>
    /// On Trigger Enter: Checks whether the other collider has a 
    ///     Chargeable component attached.
    /// There being one, it gets a reference to its Chargeable component
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        chargeableTarget = other.GetComponent<Chargeable>();

        if (chargeableTarget)
        {
            targetInRange = true;
        }
    }

    /// <summary>
    /// On Trigger Exit: Upon exiting the collision with a 
    ///     Chargeable object, nullifies targets and updates its 
    ///     internal "targetInRange" variable
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {

        chargeableTarget = other.GetComponent<Chargeable>();

        if (chargeableTarget)
        {
            targetInRange = false;
            chargeableTarget = null;
        }
    }
}
