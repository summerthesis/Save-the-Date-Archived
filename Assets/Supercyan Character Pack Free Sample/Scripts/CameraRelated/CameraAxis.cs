//MODIFIED NOVEMBER 28 BY HERCULES
//MODIFICATIONS ARE MARKED
using System.Collections;
using UnityEngine;

public class CameraAxis : MonoBehaviour
{
    [SerializeField] Transform playerAxis;
    [SerializeField] Transform aimingAxis;

    [SerializeField] float m_followingSpeed;
    bool m_isAiming = false;

    //HErC'S ADDITIONS TO VARIABLES:
    [SerializeField] private float raycastDistance;
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

    //HErC'S ADDITION
    /// <summary>
    /// function designed to fire raycast and return a transform if there's a target, null otherwise
    /// This is set to the "ThunderPhysics" layer
    /// </summary>
    /// <returns></returns>
    public Transform GetTarget() {

        //ThunderPhysics' layer is layer 8
        int thunderLayer = 1<<8;
        RaycastHit tempTarget;
        if(Physics.Raycast(cameraTransform.position, cameraTransform.transform.forward, 
                           out tempTarget, raycastDistance, thunderLayer, QueryTriggerInteraction.Ignore))
        {    
            return tempTarget.transform;
        }
        return null;
    }
    //END OF HErC'S ADDITION
}