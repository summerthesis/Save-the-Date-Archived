/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* PlayerMovement
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 26 Jan 2020 - Hyukin
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

    //** m_fMoveSpeed need to be roughly 0.12 of m_fSideMoveSpeed **
    [SerializeField] float m_fMoveSpeed;
    [SerializeField] float m_fSideMoveSpeed;
    [SerializeField] float m_fRotSpeed;

    private float m_fHorizontal;
    private float m_fVertical;    
    public float GetMoveSpeed() { return m_fMoveSpeed; }
    public float GetSideMoveSpeed() { return m_fSideMoveSpeed; }

    private void Start()
    {
        CameraBody = GameObject.Find("CameraBody");
    }

    private void FixedUpdate()
    {
        if( CameraBehaviour.GetInstance().GetIsZooming())
        {
            ZoomInModeMove();
        }
        else
        {
            PlayerFacingRot();
            MoveREgular();
        }
    }

    private void ZoomInModeMove()
    {
        m_fHorizontal = Input.GetAxis("Horizontal");
        float dt = Time.fixedDeltaTime;
        if(m_fHorizontal > 0)
        {
            transform.Translate(Vector3.right * m_fMoveSpeed * dt, Space.Self);
        }
        else if(m_fHorizontal < 0)
        {
            transform.Translate(-Vector3.right * m_fMoveSpeed * dt, Space.Self);
        }
    }

    private void MoveREgular()
    {
        m_fHorizontal = Input.GetAxis("Horizontal");
        m_fVertical = Input.GetAxis("Vertical");
        float dt = Time.fixedDeltaTime;

        if (m_fVertical != 0 && m_fHorizontal == 0)
        {
            transform.Translate(Vector3.forward * m_fMoveSpeed * dt, Space.Self);
        }
        if (m_fHorizontal > 0.2f && m_fVertical == 0)
        {
            transform.RotateAround(CameraBody.transform.position, Vector3.up, m_fSideMoveSpeed * dt);
        }
        else if (m_fHorizontal < -0.2f && m_fVertical == 0)
        {
            transform.RotateAround(CameraBody.transform.position, Vector3.up, -m_fSideMoveSpeed * dt);
        }
        else if (m_fVertical != 0 && m_fHorizontal > 0.2f)
        {
            transform.Translate(Vector3.forward * m_fMoveSpeed * 0.95f * dt, Space.Self);
            transform.RotateAround(CameraBody.transform.position, Vector3.up, m_fSideMoveSpeed * 0.03f * dt);
        }
        else if (m_fVertical != 0 && m_fHorizontal < -0.2f)
        {
            transform.Translate(Vector3.forward * m_fMoveSpeed * 0.95f * dt, Space.Self);
            transform.RotateAround(CameraBody.transform.position, Vector3.up, -m_fSideMoveSpeed * 0.03f * dt);
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
