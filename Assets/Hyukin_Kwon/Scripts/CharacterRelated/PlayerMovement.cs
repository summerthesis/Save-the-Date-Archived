/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* PlayerMovement
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 25 Jan 2020 - Hyukin
* 
* Inherits from Monobehaviour
*
* new Player behaviour
* *******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject CameraBody;

    [SerializeField] float m_fMoveSpeed;
    [SerializeField] float m_fRotSpeed;

    private float m_fHorizontal;
    private float m_fVertical;

    public float GetMoveSpeed() { return m_fMoveSpeed; }

    private void Start()
    {
        CameraBody = GameObject.Find("CameraBody");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        m_fHorizontal = Input.GetAxis("Horizontal");
        m_fVertical = Input.GetAxis("Vertical");
        float dt = Time.fixedDeltaTime;

        if (m_fVertical > 0.1f)
        {
            transform.Translate(Vector3.forward * m_fMoveSpeed * dt, Space.Self);
            transform.rotation = Quaternion.LookRotation(CameraBody.transform.forward * m_fRotSpeed * dt);
        }
        else if (m_fVertical < -0.1f)
        {
            transform.Translate(Vector3.forward * m_fMoveSpeed * dt, Space.Self);
            transform.rotation = Quaternion.LookRotation(-CameraBody.transform.forward * m_fRotSpeed * dt);
        }
    }



}
