/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* CameraAxis
* Last Modified: 27 November 2019
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

    public void SetIsAiming(bool aiming) { m_isAiming = aiming;}
    public bool GetIsAiming() { return m_isAiming; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            m_isAiming =! m_isAiming;

        if(!m_isAiming)
        {
            transform.position = Vector3.Lerp(transform.position, playerAxis.position, m_followingSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, aimingAxis.position, m_followingSpeed * Time.deltaTime);
        }
    }
}