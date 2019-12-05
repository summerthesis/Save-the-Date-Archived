/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * Gravity Control
 * Created: December 05, 2019
 * Last Modified: December 05, 2019
 * 
 * Inherits from MonoBehaviour
 * 
 * - Keeps track of the Moveable objects currently being targeted
 *      
 * - Passes information from the Camera's raycast to the 
 *   GameObject responsible for the mechanic arm's particle system
 *   
 * - UNDER IMPLEMENTATION:
 *      Functionality to lock Moveable object in the players' reticle
 * 
 ********************************************************************/
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour
{
    //Camera component for target detection
    [SerializeField] private Camera camera;
    private CameraAxis cameraAxis; //new target tracking functionality
    public bool Aiming { get { return cameraAxis.GetIsAiming(); } }
    private Transform targetTransform;
    public Transform TargetTransform { get { return cameraAxis.GetTarget(); } }

    private Transform innerTarget;
    [SerializeField] Vector3 innerTargetPos;
    [SerializeField] Quaternion innerTargetRotation;
    
    /// <summary>
    /// Script's initialization functionality
    /// </summary>
    void Awake()
    {
        targetTransform = null;
        innerTarget = null;
    }

    // Start is called before the first frame update
    /// <summary>
    /// Gets Camera Axis component from Camera
    /// </summary>
    void Start()
    {
        cameraAxis = camera.GetComponentInParent<CameraAxis>();
    }

    // Update is called once per frame
    /// <summary>
    /// Simply finds the target's transform and calls the Levitate function
    /// </summary>
    void Update()
    {
        innerTarget = this.gameObject.GetComponent<Transform>();

        innerTargetPos = innerTarget.position;
        innerTargetRotation = innerTarget.rotation;

        targetTransform = cameraAxis.GetTarget();

        if (targetTransform)
        {
            Levitate(Input.GetKey(KeyCode.Q) || Input.GetButton("Fire3"));
        }
    }

    /// <summary>
    /// Levitate function. Will have to be refactored
    /// The current implementation allows for weird stuff to happen...
    /// </summary>
    /// <param name="state"></param>
    void Levitate(bool state)
    {
        Moveable tempMov = targetTransform.GetComponent<Moveable>();
        if (tempMov)
        {
            if (state)
            {
                tempMov.SetAfloat(targetTransform);
            }
            else
            {
                tempMov.SetAfloat(null);
            }
        }
    }
}
