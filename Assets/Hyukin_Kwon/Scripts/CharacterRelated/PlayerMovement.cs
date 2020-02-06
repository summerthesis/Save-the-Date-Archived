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
    private PlayerFoot PlayerFoot;

    //** m_fMoveSpeed need to be roughly 0.12 of m_fSideMoveSpeed **
    [SerializeField] float m_fMoveSpeed;
    [SerializeField] float m_fSideMoveSpeed;
    [SerializeField] float m_fRotSpeed;

    [SerializeField] bool m_bIsGrounded = true;
    [SerializeField] float m_JumpForce;

    private float m_fHorizontal;
    private float m_fVertical;
    private Quaternion qTo;
    private float fdt;

    private Rigidbody rigid;

    public float GetMoveSpeed() { return m_fMoveSpeed; }
    public float GetSideMoveSpeed() { return m_fSideMoveSpeed; }

    private void Start()
    {
        CameraBody = GameObject.Find("CameraBody");
        PlayerFoot = GameObject.Find("PlayerFoot").GetComponent<PlayerFoot>();
        rigid = GetComponent<Rigidbody>();
        qTo = transform.rotation;
    }

    private void FixedUpdate()
    {
        m_fHorizontal = Input.GetAxis("Horizontal");
        m_fVertical = Input.GetAxis("Vertical");
        fdt = Time.fixedDeltaTime;

        if ( CameraBehaviour.GetInstance().GetIsZooming())
        {
            ZoomInModeMove();
        }
        else
        {
            PlayerFacingRot();
            MoveRegular();
            JumpRegular();
        }
    }

    private void JumpRegular()
    {
        //m_bIsGrounded = PlayerFoot.GetIsGrounded();
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1)) )//&& m_bIsGrounded)
        {
            Debug.Log("Jump");
            rigid.AddForce(Vector3.up * m_JumpForce);
            PlayerFoot.SetIsGrounded(false);

            if (m_fVertical != 0)
            {
                rigid.AddForce(transform.forward * Mathf.Abs(m_fVertical) * m_JumpForce * 0.3f);
            }     
            else if (m_fHorizontal != 0)
            {
                rigid.AddForce(transform.forward * Mathf.Abs(m_fHorizontal) * m_JumpForce * 0.3f);
            }
        }
    }

    private void ZoomInModeMove()
    {
        //if (!m_bIsGrounded) return;

        if (m_fHorizontal > 0 && m_fVertical == 0)
            transform.Translate(Vector3.right * m_fMoveSpeed * fdt, Space.Self);
        else if(m_fHorizontal < 0 && m_fVertical == 0)
            transform.Translate(-Vector3.right * m_fMoveSpeed * fdt, Space.Self);

        if(m_fVertical > 0 && m_fHorizontal == 0)
            transform.Translate(Vector3.forward * m_fMoveSpeed * fdt, Space.Self);
        else if (m_fVertical < 0 && m_fHorizontal == 0)
            transform.Translate(-Vector3.forward * m_fMoveSpeed * fdt, Space.Self);
    }

    private void MoveRegular()
    {
        //if (!m_bIsGrounded) return;

        if (m_fVertical != 0 && m_fHorizontal == 0)
        {            
            transform.Translate(Vector3.forward * m_fMoveSpeed * fdt, Space.Self);
        }
        if (m_fHorizontal > 0.2f && m_fVertical == 0)
        {
            transform.RotateAround(CameraBody.transform.position, Vector3.up, m_fSideMoveSpeed * fdt);
        }
        else if (m_fHorizontal < -0.2f && m_fVertical == 0)
        {
            transform.RotateAround(CameraBody.transform.position, Vector3.up, -m_fSideMoveSpeed * fdt);
        }
        else if (m_fVertical != 0 && m_fHorizontal > 0.2f)
        {
            transform.Translate(Vector3.forward * m_fMoveSpeed * 0.95f * fdt, Space.Self);
            transform.RotateAround(CameraBody.transform.position, Vector3.up, m_fSideMoveSpeed * 0.03f * fdt);
        }
        else if (m_fVertical != 0 && m_fHorizontal < -0.2f)
        {
            transform.Translate(Vector3.forward * m_fMoveSpeed * 0.95f * fdt, Space.Self);
            transform.RotateAround(CameraBody.transform.position, Vector3.up, -m_fSideMoveSpeed * 0.03f * fdt);
        }
    }

    private void PlayerFacingRot()
    {
        //if (!m_bIsGrounded) return;

        if (m_fHorizontal > 0.2f && m_fVertical > 0.1f)
            qTo = Quaternion.LookRotation((CameraBody.transform.forward + CameraBody.transform.right).normalized);
        else if (m_fHorizontal > 0.2f && m_fVertical < -0.1f)
            qTo = Quaternion.LookRotation((-CameraBody.transform.forward + CameraBody.transform.right).normalized);
        else if (m_fHorizontal < -0.2f && m_fVertical > 0.1f)
            qTo = Quaternion.LookRotation((CameraBody.transform.forward - CameraBody.transform.right).normalized);
        else if (m_fHorizontal < -0.2f && m_fVertical < -0.1f)
            qTo = Quaternion.LookRotation((-CameraBody.transform.forward - CameraBody.transform.right).normalized);
        else if (m_fHorizontal > 0.2f && m_fVertical == 0)
            qTo = Quaternion.LookRotation(CameraBody.transform.right);
        else if (m_fHorizontal < -0.2f && m_fVertical == 0)
            qTo = Quaternion.LookRotation(-CameraBody.transform.right);
        else if (m_fHorizontal == 0 && m_fVertical > 0.1f)
            qTo = Quaternion.LookRotation(CameraBody.transform.forward);
        else if (m_fHorizontal == 0 && m_fVertical < -0.1f)
            qTo = Quaternion.LookRotation(-CameraBody.transform.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, qTo, m_fRotSpeed * fdt);
    }

}
