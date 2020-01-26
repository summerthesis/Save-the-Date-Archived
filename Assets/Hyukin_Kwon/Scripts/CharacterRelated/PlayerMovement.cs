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
    [SerializeField] float m_fSideMoveSpeed;
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
        Vector3 speed = Vector3.zero;
        m_fHorizontal = Input.GetAxis("Horizontal");
        m_fVertical = Input.GetAxis("Vertical");
        float dt = Time.fixedDeltaTime;

        PlayerFacingRot();

        if (m_fVertical > 0.1f || m_fVertical < -0.1f)
        {
            speed += Vector3.forward * m_fMoveSpeed * dt;
            transform.Translate(Vector3.forward * m_fMoveSpeed * dt, Space.Self);
        }
        if (m_fHorizontal > 0.2f)
        {
            transform.RotateAround(CameraBody.transform.position, Vector3.up, m_fSideMoveSpeed * dt);
        }
        else if (m_fHorizontal < -0.2f)
        {
            transform.RotateAround(CameraBody.transform.position, Vector3.up, -m_fSideMoveSpeed * dt);
        }
    }

    private void PlayerFacingRot()
    {       
        float dt = Time.fixedDeltaTime;
        if (m_fHorizontal > 0.2f && m_fVertical > 0.1f)
            transform.rotation = Quaternion.LookRotation((CameraBody.transform.forward + CameraBody.transform.right).normalized * m_fRotSpeed * dt);
        else if (m_fHorizontal > 0.2f && m_fVertical < -0.1f)
            transform.rotation = Quaternion.LookRotation((-CameraBody.transform.forward + CameraBody.transform.right).normalized * m_fRotSpeed * dt);
        else if (m_fHorizontal < -0.2f && m_fVertical > 0.1f)
            transform.rotation = Quaternion.LookRotation((CameraBody.transform.forward - CameraBody.transform.right).normalized * m_fRotSpeed * dt);
        else if (m_fHorizontal < -0.2f && m_fVertical < -0.1f)
            transform.rotation = Quaternion.LookRotation((-CameraBody.transform.forward - CameraBody.transform.right).normalized * m_fRotSpeed * dt);
        else if (m_fHorizontal > 0.2f && m_fVertical == 0)
            transform.rotation = Quaternion.LookRotation(CameraBody.transform.right * m_fRotSpeed * dt);
        else if (m_fHorizontal < -0.2f && m_fVertical == 0)
            transform.rotation = Quaternion.LookRotation(-CameraBody.transform.right * m_fRotSpeed * dt);
        else if (m_fHorizontal == 0 && m_fVertical > 0.1f)
            transform.rotation = Quaternion.LookRotation(CameraBody.transform.forward * m_fRotSpeed * dt);
        else if (m_fHorizontal == 0 && m_fVertical < -0.1f)
            transform.rotation = Quaternion.LookRotation(-CameraBody.transform.forward * m_fRotSpeed * dt);
    }

}
