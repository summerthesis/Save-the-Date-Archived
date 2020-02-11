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
    private GameObject CamPivot;
    [SerializeField] float m_fCampPivotDis;

    //** m_fMoveSpeed need to be roughly 0.12 of m_fSideMoveSpeed **
    [SerializeField] float m_fMoveSpeed;
    private Vector3 m_fOverallSpeed = Vector3.zero;
    [SerializeField] float m_fRotSpeed;

    [SerializeField] bool m_bIsGrounded = true;
    [SerializeField] float m_JumpForce;
    

    private float m_fHorizontal;
    private float m_fVertical;
    private Quaternion qTo;
    private float fdt;

    private Rigidbody rigid;

    public float GetMoveSpeed() { return m_fMoveSpeed; }

    public Vector3 GetOverallSpeed() { return m_fOverallSpeed; }

    private void Start()
    {
        CameraBody = GameObject.Find("CameraBody");
        PlayerFoot = GameObject.Find("PlayerFoot").GetComponent<PlayerFoot>();
        CamPivot = GameObject.Find("CamPivot");
        m_fCampPivotDis = CameraBehaviour.GetInstance().GetDistance();
        rigid = GetComponent<Rigidbody>();
        qTo = transform.rotation;
    }

    private void FixedUpdate()
    {
        m_fHorizontal = Input.GetAxis("Horizontal");
        m_fVertical = Input.GetAxis("Vertical");
        fdt = Time.fixedDeltaTime;

        SetCampPivotPos();

        if (CameraBehaviour.GetInstance().GetIsZooming())
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

    private void SetCampPivotPos()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.forward), out hit, m_fCampPivotDis))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.forward) * hit.distance, Color.yellow);
            CamPivot.transform.localPosition = Vector3.Lerp(CamPivot.transform.localPosition, new Vector3(0, 0, -hit.distance), 20 * fdt);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.forward) * m_fCampPivotDis, Color.cyan);
            CamPivot.transform.localPosition = Vector3.Lerp(CamPivot.transform.localPosition, new Vector3(0, 0, -m_fCampPivotDis), 20 * fdt);
        }
    }

    private void JumpRegular()
    {
        //m_bIsGrounded = PlayerFoot.GetIsGrounded();
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1)))//&& m_bIsGrounded)
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
        if (!m_bIsGrounded) return;
        Vector3 speed = Vector3.zero;
        speed = (Vector3.right * m_fHorizontal) + (Vector3.forward * m_fVertical);
        speed.Normalize();
        speed *= m_fMoveSpeed;
        transform.Translate(speed * fdt, Space.Self);
    }

    private void MoveRegular()
    {
        //if (!m_bIsGrounded) return;

        if (m_fVertical == 0 && m_fHorizontal != 0)
            m_fOverallSpeed.x = m_fHorizontal * m_fMoveSpeed * 10;
        else if (m_fVertical != 0 && m_fHorizontal == 0)
            m_fOverallSpeed.z = Mathf.Abs(m_fVertical) * m_fMoveSpeed;
        else if (m_fVertical != 0 && m_fHorizontal != 0)
        {
            m_fOverallSpeed.x = m_fHorizontal * m_fMoveSpeed * 2;
            m_fOverallSpeed.z = Mathf.Abs(m_fVertical) * m_fMoveSpeed * 5;
        }
        m_fOverallSpeed = m_fOverallSpeed.normalized;
        //Debug.Log(m_fOverallSpeed);

        if (m_fVertical != 0 && m_fHorizontal == 0)
        {
            transform.Translate(Vector3.forward * m_fOverallSpeed.z * m_fMoveSpeed * fdt, Space.Self);
        }
        if (m_fHorizontal != 0 && m_fVertical == 0)
        {
            transform.RotateAround(CameraBody.transform.position, Vector3.up, m_fMoveSpeed * m_fOverallSpeed.x * 8 * fdt);
        }
        else if (m_fVertical != 0 && m_fHorizontal != 0)
        {
            transform.Translate(Vector3.forward * m_fOverallSpeed.z * m_fMoveSpeed * fdt, Space.Self);
            transform.RotateAround(CameraBody.transform.position, Vector3.up, m_fMoveSpeed * m_fOverallSpeed.x * 8 * fdt);
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
