/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* PlayerMovement
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 18 Feb 2020 - Hyukin
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
    private GameObject FrontHeightCheck;
    private Animator anim;

    //** m_fMoveSpeed need to be roughly 0.12 of m_fSideMoveSpeed **
    [SerializeField] float m_fMoveSpeed;
    private Vector3 m_fOverallSpeed = Vector3.zero;
    [SerializeField] float m_fRotSpeed;

    [SerializeField] bool m_bIsGrounded = true;
    [SerializeField] float m_JumpForce;
    private float m_fDisToGround = 0.0f;
    private float m_fFrontDisToGround = 0.0f;

    [SerializeField] bool m_bIsOnStair = false;
    [SerializeField] bool m_bIsStairInFront = false;

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
        FrontHeightCheck = GameObject.Find("FrontHeightCheck");
        rigid = GetComponent<Rigidbody>();
        qTo = transform.rotation;
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        m_fHorizontal = movementInput.x;
        m_fVertical = movementInput.y;
        fdt = Time.fixedDeltaTime;

        anim.SetFloat("HSpeed", Mathf.Abs(m_fHorizontal));
        anim.SetFloat("VSpeed", Mathf.Abs(m_fVertical));

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
            CamPivot.transform.localPosition = Vector3.Lerp(CamPivot.transform.localPosition, new Vector3(0, 0, -hit.distance), 20 * fdt);
            if (hit.transform.tag != "Camera")
                Debug.Log(hit.transform.gameObject);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * distance, Color.cyan);
            CamPivot.transform.localPosition = Vector3.Lerp(CamPivot.transform.localPosition, new Vector3(0, 0, -distance), 20 * fdt);
        }
    }

    private void JumpRegular()
    {
        anim.SetBool("Jump", !m_bIsGrounded);

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
        m_bIsOnStair = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDis))
        {
            Debug.DrawRay(transform.position, Vector3.down * rayDis, Color.green);
            m_bIsGrounded = true;
            if (hit.transform.tag == "Stair")
                m_bIsOnStair = true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * rayDis, Color.red);
            if(!CameraBehaviour.GetInstance().zoomInput)
                m_bIsGrounded = false;
        }

        if (m_bIsOnStair && Physics.Raycast(FrontHeightCheck.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            m_bIsStairInFront = true;
            m_fFrontDisToGround = hit.distance;
        }

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            m_fDisToGround = hit.distance;
        }

        Debug.Log("m_fDisToGround: " + m_fDisToGround + ", m_fFrontDisToGround: " + m_fFrontDisToGround);
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