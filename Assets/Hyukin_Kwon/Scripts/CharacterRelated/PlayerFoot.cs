/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* PlayerFoot
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 11 Feb 2020 - Hyukin
* 
* Inherits from Monobehaviour
* *******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    private bool m_bIsGrounded;
    [SerializeField] float rayDis;

    public bool GetIsGrounded() { return m_bIsGrounded; }
    public void SetIsGrounded(bool b) { m_bIsGrounded = b; }

    private void Update()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, rayDis))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * rayDis, Color.green);
            if (hit.transform.tag != "Player")
            {
                m_bIsGrounded = true;
            }

        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * rayDis, Color.red);
        }
    }
}
