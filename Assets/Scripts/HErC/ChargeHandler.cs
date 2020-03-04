/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * Charge Handler
 * Created:       Nov 14, 2019
 * Last Modified: Feb 25, 2019
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
 *   //CHANGES IN FEB 09:
 * - Removed ThunderCaster from Prefab. Particle system will have to be
 *      implemented by artists
 * - Reworked some of the functionality related to the camera and control 
 *      system, since they were overhauled
 *  
 *  //CHANGES IN FEB 18&19
 * - Reworked the controls to match Unity's new system
 * - Completely removed camera reference
 * 
 *  //CHANGES IN FEB 24&25
 * - Reworked detection system to match the initial, range-based design
 * - Implemented test functionality for the haptics feedback.
 * - (OBS: This test shall be moved to a different script)
 * - CURRENT ISSUES:
 *      Chargeable script not being found in children.
 *      
 * 
 ********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ChargeTracker))]
public class ChargeHandler : MonoBehaviour
{
    [SerializeField] private Transform m_characterTransform;
    //Charge Tracking variables
    private ChargeTracker chargeTracker;
    public int MaxCharges { get { return chargeTracker.MaxCharges; } }
    public int Charges { get { return chargeTracker.Charges; } }

    private PlayerInputAction m_chargeAction;
    private bool m_bChargeExchange;

    //Serialized for visualization purposes only
    [SerializeField] private Transform targetTransform; 

    private SphereCollider m_sphereCol;

    //TEST FUNCTIONALITY. WILL BE MOVED
    [SerializeField] private bool m_bFlipMotors;
    Gamepad m_pad;

    /// <summary>
    /// Script init functions. Gets references to the components and targets
    /// </summary>
    void Awake()
    {
        targetTransform = null;
        chargeTracker = this.gameObject.GetComponent<ChargeTracker>();

        m_chargeAction = new PlayerInputAction();
        
        //m_chargeAction.PlayerControls.ElecArm.started += ctx => m_bChargeExchange = true;
        m_chargeAction.PlayerControls.ElecArm.performed += ctx => m_bChargeExchange = true;
        m_chargeAction.PlayerControls.ElecArm.canceled += ctx => m_bChargeExchange = false;

        m_sphereCol = this.gameObject.GetComponent<SphereCollider>();

        //TEST
        m_pad = Gamepad.current;
        if (m_pad != null) m_pad.SetMotorSpeeds(0.1f, 0.0f);
    }

    /// <summary>
    /// Does nothing anymore
    /// </summary>
    void Start()
    {
        if (m_sphereCol) { m_sphereCol.radius = CameraBehaviour.GetInstance().GetElectricDistance(); }
        //TEST

        if (m_pad != null) m_pad.PauseHaptics();
    }

    /// <summary>
    /// Performs two basic operations:
    /// 1. Checks for a target in range (from Target Tracker)
    /// 2. If the player presses a button with the target in range, it exchanges the charges
    /// 3. If there's no target in range, it nullifies the corresponding transforms
    /// </summary>
    void Update()
    {
        this.gameObject.transform.position = m_characterTransform.position;

        //targetTransform = CameraBehaviour.GetInstance().GetTarget(); // modified by Hyukin
       
        if (targetTransform)
        {
            float tempL = 1-((targetTransform.position - this.gameObject.transform.position).magnitude/m_sphereCol.radius);
            float tempR = 0.1f;

            if (m_bFlipMotors) { m_pad.SetMotorSpeeds(tempL, tempR); }
            else { m_pad.SetMotorSpeeds(tempR, tempL); }
            
            if (m_bChargeExchange)
            {
                m_bChargeExchange = false;
                ExchangeCharges();
            }
        }
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

        Chargeable temp = targetTransform.GetComponent<Chargeable>();
        if (!temp) temp = targetTransform.GetComponentInChildren<Chargeable>();
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
        chargeTracker.Discharge();
        targetTransform.GetComponent<Chargeable>().Charge();
    }

    private void OnEnable()
    {
        m_chargeAction.Enable();
    }

    private void OnDisable()
    {
        if(m_pad != null) m_pad.ResetHaptics();
        m_chargeAction.Disable();
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.name);
        Chargeable temp = other.gameObject.GetComponent<Chargeable>();
        if (!temp) temp = other.gameObject.GetComponentInChildren<Chargeable>();
        if (temp) {
            Debug.Log(temp.gameObject.name);
            if (!targetTransform) {
                targetTransform = other.gameObject.transform;
            }
            else
            {
                Debug.Log("There's already another target in range...");
            }
            m_pad.ResumeHaptics();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<Chargeable>() != null ||
            other.gameObject.GetComponentInChildren<Chargeable>() != null) {
            targetTransform = null;
            m_pad.PauseHaptics();
        }
    }
}
