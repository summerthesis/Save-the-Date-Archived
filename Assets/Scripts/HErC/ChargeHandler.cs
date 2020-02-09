/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * Charge Handler
 * Created: November 14, 2019
 * Last Modified: November 28, 2019
 * 
 * Inherits from MonoBehaviour
 * 
 * - Keeps track of the context in which charges are exchanged
 *   between the mechanical arm and chargeable objects.
 *   
 * - Requires access to the Camera Axis script, to extract aiming info
 *   
 * - Also works assuming there is a particle system emitter
 *   attached as a child GameObject. Said child needs to have a
 *   "Thunder Caster" script attached.
 *   
 * - Passes information from the Camera's raycast to the
 *   GameObject responsible for the mechanic arm's particle system
 *   
 *   //CHANGES IN NOV 28:
 * - Included pass-through functionality for the tracking management
 *      This is what the canvas uses to display charges and aiming
 *      
 * - Removed dependence on Target Tracker, due to target detection changes
 * 
 *   //CHANGES IN DEC 05:
 * - Removed Gravity Gun functionality from this script, moved it to the
 *      GravityControl class
 * 
 ********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChargeTracker))]
public class ChargeHandler : MonoBehaviour
{
    //Charge Tracking variables
    private ChargeTracker chargeTracker;
    public int MaxCharges { get { return chargeTracker.MaxCharges; } }
    public int Charges { get { return chargeTracker.Charges; } }

    //Camera component for target detection
    [SerializeField] private Camera camera;
    //private CameraAxis cameraAxis; //new target tracking functionality - [DOESN'T NEED IT ANYMORE SINCE] CameraMovement is Singleton - Hyukin
    //public bool Aiming { get { return cameraAxis.GetIsAiming(); } } - [DOESN'T NEED IT ANYMORE SINCE] CameraMovement is Singleton - Hyukin
    private Transform targetTransform;
    // public Transform TargetTransform { get { return cameraAxis.GetTarget(); } } - [DOESN'T NEED IT ANYMORE SINCE] CameraMovement is Singleton - Hyukin

    //Particle system manager
    private ThunderCaster thunderCaster; //(child's) thunder caster component

    /// <summary>
    /// Script init functions. Gets references to the components and targets
    /// </summary>
    void Awake()
    {
        targetTransform = null;
        //targetTracker = this.gameObject.GetComponent<TargetTracker>();
        chargeTracker = this.gameObject.GetComponent<ChargeTracker>();
    }

    /// <summary>
    /// Gets the Thunder Caster component from the child
    /// </summary>
    void Start()
    {
        //may require some sort of assertion
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
        targetTransform = CameraMovement.GetInstance().GetTarget(); // modified by Hyukin

        UpdateTarget();

        if (targetTransform)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire2"))
            {
                ExchangeCharges();
            }
        }
    }

    void UpdateTarget()
    {
        thunderCaster.SetTarget(targetTransform);
    }

    /// <summary>
    /// Context for the charge exchange operation:
    /// - Checks for the validity of a chargeable object, to avoid
    ///     null exceptions
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

        Chargeable temp =targetTransform.GetComponent<Chargeable>();
        if (temp)
        {
            if (temp.Charged && chargeTracker.CanRecharge)
            {
                Absorb();
            }
            else if (temp.Charged && !chargeTracker.CanRecharge)
            {
                //ping error: both full
            }
            else if (!temp.Charged && chargeTracker.CanDischarge)
            {
                Charge();
            }
            else if (!temp.Charged && !chargeTracker.CanDischarge)
            {
                //ping error: both empty
            }
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
        targetTransform.GetComponent<Chargeable>().ReturnCharge();
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
        targetTransform.GetComponent<Chargeable>().Charge();
    }
}
