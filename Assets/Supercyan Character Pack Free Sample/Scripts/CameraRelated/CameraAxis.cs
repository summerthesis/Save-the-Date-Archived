/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* CameraAxis
* Last Modified: 29 November 2019 by Hercules
* 
* Inherits from Monobehaviour
*
*
* - Keep the playerAxis (postion)
* - Keep the aimingAxis (Player's shoulder position)
* *******************************************************/

using System.Collections;
using UnityEngine;

public class CameraAxis : MonoBehaviour
{
    [SerializeField] Transform playerAxis;
    [SerializeField] Transform aimingAxis;

    [SerializeField] float m_followingSpeed;
    bool m_isAiming = false;

    //HErC'S ADDITIONS TO VARIABLES:
    [SerializeField] private GameObject gravityTarget;
    [SerializeField] private float raycastDistance;
    [SerializeField] private float raycastFactor;
    public float RaycastDistance { get { return raycastDistance; } }
    private Transform cameraTransform;
    //END OF HErC'S ADDITIONS

    public void SetIsAiming(bool aiming) { m_isAiming = aiming;}
    public bool GetIsAiming() { return m_isAiming; }

    //HErC'S ADDITION
    void Start()
    {
        cameraTransform = this.gameObject.GetComponentInChildren<Transform>();
    }
    //END OF HErC's ADDITION

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
            m_isAiming = !m_isAiming;

        if(!m_isAiming)
        {
            transform.position = Vector3.Lerp(transform.position, playerAxis.position, m_followingSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, aimingAxis.position, m_followingSpeed * Time.deltaTime);
            
            GetTarget();//HErC's addition
        }
    }

    //HErC'S ADDITIONS
    /// <summary>
    /// function designed to fire raycast and return a transform if there's a target, null otherwise
    /// This is set to the "ThunderPhysics" layer
    /// </summary>
    /// <returns></returns>
    public Transform GetTarget() {

        //ThunderPhysics' layer is 8
        //GravityPhysics' layer is 9
        int layers = ((1<<8) + (1<<9));
        RaycastHit tempTarget;
        if(Physics.Raycast(cameraTransform.position, cameraTransform.transform.forward, 
                           out tempTarget, raycastDistance, layers, QueryTriggerInteraction.Ignore))
        {    
            return tempTarget.transform;
        }
        return null;
    }

    /// <summary>
    /// This function is supposed to return a placeholder transform for the gravity control scheme
    /// </summary>
    /// <returns></returns>
    public Transform GravityTarget() {

        if (m_isAiming) {

            gravityTarget.transform.position = 
                this.gameObject.transform.position + 
                (this.gameObject.transform.forward * (raycastDistance * raycastFactor));
            gravityTarget.transform.rotation = this.gameObject.transform.rotation;
        }

        return gravityTarget.transform;
    }
    //END OF HErC'S ADDITIONS
}