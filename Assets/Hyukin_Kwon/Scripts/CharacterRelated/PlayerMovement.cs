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
* Stair need to be tagged with "Stair"
* *******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject CameraBody;
    private GameObject CamPivot;

    //** m_fMoveSpeed need to be roughly 0.12 of m_fSideMoveSpeed **
    [SerializeField] float m_fMoveSpeed;
    private Vector3 m_fOverallSpeed = Vector3.zero;
    [SerializeField] float m_fRotSpeed;

    [SerializeField] bool m_bIsGrounded = true;
    [SerializeField] float m_JumpForce;
    private float m_fDisToGround = 0.0f;

    [SerializeField] bool m_bIsOnStair = false;

    private float m_fHorizontal;
    private float m_fVertical;
    private Quaternion qTo;
    private float fdt;

    private Rigidbody rigid;
    [SerializeField] float rayDis; //for ground checking

    #region Player Input Action
    PlayerInputAction inputAction;
    public Vector2 movementInput;
    public bool jumpInput;

    #endregion

    #region SetterAndGetter

    public void SetIsGround(bool isGround) { m_bIsGrounded = isGround; }
    public bool GetIsGround() { return m_bIsGrounded; }
    public void SetIsOnStair(bool isStair) { m_bIsOnStair = isStair; }
    public bool GetIsOnStair() { return m_bIsOnStair; }
    public float GetMoveSpeed() { return m_fMoveSpeed; }
    public Vector3 GetOverallSpeed() { return m_fOverallSpeed; }
    public float GetDisToGround() { return m_fDisToGround; }

    #endregion

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        inputAction.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputAction.PlayerControls.Jump.performed += ctx => jumpInput = true;
        inputAction.PlayerControls.Jump.canceled += ctx => jumpInput = false;
    }

    private void Start()
    {
        CameraBody = GameObject.Find("CameraBody");
        CamPivot = GameObject.Find("CamPivot");
        rigid = GetComponent<Rigidbody>();
        qTo = transform.rotation;
    }

    private void FixedUpdate()
    {
        m_fHorizontal = movementInput.x;
        m_fVertical = movementInput.y;
        fdt = Time.fixedDeltaTime;

        CheckGround();
        if (CameraBehaviour.GetInstance().GetIsZooming())
        {
            ZoomInModeMove();
        }
        else
        {
            SetCampPivotPos();
            PlayerFacingRot();
            MoveRegular();
            JumpRegular();
        }
    }

    private void SetCampPivotPos()
    {
        RaycastHit hit;
        float distance = CameraBehaviour.GetInstance().m_fDistanceOrigin;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, distance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hit.distance, Color.yellow);
            CamPivot.transform.localPosition = Vector3.Lerp(CamPivot.transform.localPosition, new Vector3(0, 0, -hit.distance + 0.3f), 20 * fdt);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * distance, Color.cyan);
            CamPivot.transform.localPosition = Vector3.Lerp(CamPivot.transform.localPosition, new Vector3(0, 0, -distance), 20 * fdt);
        }

        Vector3 dir = (CameraBehaviour.GetInstance().transform.position - transform.position).normalized;
        distance = Vector3.Distance(transform.position,CameraBehaviour.GetInstance().transform.position);
        Debug.DrawRay(transform.position, dir * distance, Color.red);
        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            //CameraBehaviour.GetInstance().heightFromPlayer  = hit
        }

    }

    private void JumpRegular()
    {
        if (jumpInput && m_bIsGrounded)
        {
            rigid.AddForce(Vector3.up * m_JumpForce);

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
    private void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDis))
        {
            Debug.DrawRay(transform.position, Vector3.down * rayDis, Color.green);
            m_bIsGrounded = true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * rayDis, Color.red);
            if(!CameraBehaviour.GetInstance().zoomInput)
                m_bIsGrounded = false;
        }

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            m_fDisToGround = hit.distance;
        }
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }
}