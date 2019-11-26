/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * Charge Handler
 * Created: November 14, 2019
 * Last Modified: November 18, 2019
 * 
 * Inherits from MonoBehaviour
 * 
 * - Keeps track of the context in which charges are exchanged
 *   between the mechanical arm and chargeable objects.
 *   
 * - Requires that the containing GameObject have
 *   both a Target Tracker and a ChargeTracker attached to it
 *   
 * - Also works assuming there is a particle system emitter
 *   attached as a child GameObject. Said child needs to have a
 *   "Thunder Caster" script attached.
 *   
 * - Passes information from the Target Tracker component to the 
 *   GameObject responsible for the mechanic arm's particle system
 * 
 ********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChargeTracker))]
[RequireComponent(typeof(TargetTracker))]
public class ChargeHandler : MonoBehaviour
{
    
    private ChargeTracker chargeTracker; //charge tracker component

    private TargetTracker targetTracker; //target tracker component
    private Transform targetTransform; //target's transform, should there be one
    public Transform TargetTransform { get { return targetTransform; } }

    private ThunderCaster thunderCaster; //(child's) thunder caster component

    private Light tempLight; //Temporary light: will be refactored

    /// <summary>
    /// Script init functions. Gets references to the components and targets
    /// </summary>
    void Awake()
    {
        targetTransform = null;
        targetTracker = this.gameObject.GetComponent<TargetTracker>();
        chargeTracker = this.gameObject.GetComponent<ChargeTracker>();

        tempLight = this.gameObject.GetComponent<Light>();
    }

    /// <summary>
    /// Gets the Thunder Caster component from the child
    /// </summary>
    void Start()
    {
        thunderCaster = this.gameObject.GetComponentInChildren<ThunderCaster>();
    }

    /// <summary>
    /// Performs two basic operations:
    /// 1. Checks for a target in range (from Target Tracker)
    /// 2. There being a target, updates the Thunder Caster's target information
    ///     If the player presses a button with the target in range, 
    ///     it exchanges the charges
    /// 3. If there's no target in range, it nullifies the corresponding transforms
    /// </summary>
    void Update()
    {
        //This line of code just turns light on/off. Will be removed later
        tempLight.enabled = targetTracker.TargetInRange;

        if (targetTracker.TargetInRange)
        {
            UpdateTarget();

            if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire2"))
            {
                ExchangeCharges();
            }
        }
        else
        {
            NullifyTarget();
        }
    }

    void UpdateTarget()
    {
        targetTransform = targetTracker.Charger.transform;
        thunderCaster.SetTarget(targetTransform);
    }

    void NullifyTarget()
    {
        targetTransform = null;
        thunderCaster.SetTarget(targetTransform);
    }

    /// <summary>
    /// Context for the charge exchange operation:
    /// - Checks whether for charges on both the player and the target
    /// - Performs comparisons to see whether the target can be charged,
    ///     and whether the player can charge the carget or absorb charges
    /// - Both conditions being fulfilled, it performs the charge exchange
    ///     by calling the corresponding functions in the related components/classes
    /// - If it is not possible to fulfill the conditions, it does nothing (as of now)
    /// 
    /// - The comparison order is as follows:
    ///     Chargeable is charged and player can absorb charges        -> absorb
    ///     Chargeable is charged and player cannot absorb charges     -> do nothing
    ///     Chargeable is not charged and player can absorb charges    -> charge
    ///     Chargeable is not charged and player cannot absorb charges -> do nothing
    /// </summary>
    void ExchangeCharges() {

        if (targetTracker.Charger.Charged && chargeTracker.CanRecharge) {
            Absorb();
        }
        else if (targetTracker.Charger.Charged && !chargeTracker.CanRecharge) {
            //ping error: both full
        }
        else if (!targetTracker.Charger.Charged && chargeTracker.CanDischarge) {
            Charge();
        }
        else if(!targetTracker.Charger.Charged && !chargeTracker.CanDischarge){
            //ping error: both empty
        }
    }

    /// <summary>
    /// Calls the three components in the following order:
    /// 1. Fires thunder particle system in child GameObject
    /// 2. Calls the ReturnCharge() function in the chargeable object
    /// 3. Handles Recharge() result in the mechanical arm charge tracker
    /// </summary>
    void Absorb()
    {
        thunderCaster.CastThunder();
        targetTracker.Charger.ReturnCharge();
        chargeTracker.Recharge();
    }

    /// <summary>
    /// Calls the three components in the following order:
    /// 1. Fires thunder particle system in child GameObject
    /// 2. Handles the Discharge() result in the mechanical arm charge tracker
    /// 3. Calls the Charge() function in the chargeable object
    /// </summary>
    void Charge()
    {
        thunderCaster.CastThunder();
        chargeTracker.Discharge();
        targetTracker.Charger.Charge();
    }
}
