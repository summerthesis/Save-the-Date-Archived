/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* CameraMovement
* Modified: 27 November 2019
* Last Modified: 17 Jan 2020
* 
* Inherits from Monobehaviour
*
*
* - Do basic camera movement here
* - Singleton pattern is implemented
* - combined CameraMovement.cs and CameraAxis.cs
* - disabling cam rotation whein is_aiming == true
* *******************************************************/

using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region original varibles
    //for zoom over the shoulder variables
    [SerializeField] float m_aimingZoomSpeed;
    [SerializeField] float m_aimingZoomRotSpeed;
    Transform m_saveCameraTrans;
    bool m_isChangingState = false;
    Vector3 m_saveEulerAngle;

    //regular camera movement variables
    [SerializeField] float m_distance;
    [SerializeField] float m_rotSpeed;
    [SerializeField] float m_zoomSpeed; 

    [SerializeField] float m_minHeight;
    [SerializeField] float m_maxHeight;
    [SerializeField] float m_minDistance;
    [SerializeField] float m_maxDistance;

    float m_mouseX;
    float m_mouseY;

    Vector3 m_eulerAngle;
    #endregion

    #region variables from CameraAxis.cs

    [SerializeField] Transform playerAxis;
    [SerializeField] Transform aimingAxis;

    [SerializeField] float m_followingSpeed;
    [SerializeField] bool m_isAiming = false;
    [SerializeField] float m_sideMovingSpeed;
    [SerializeField] float m_leftMovingRange;
    [SerializeField] float m_rightMovingRange;
    Vector3 leftSideMovingLimit;
    Vector3 rightSideMovingLimit;

    //HErC'S ADDITIONS TO VARIABLES:
    [SerializeField] private GameObject gravityTarget;
    [SerializeField] private float raycastDistance;
    [SerializeField] private float raycastFactor;

    public float RaycastDistance { get { return raycastDistance; } }
    private Transform cameraTransform;
    //END OF HErC'S ADDITIONS

    public void SetIsAiming(bool aiming) { m_isAiming = aiming; }
    public bool GetIsAiming() { return m_isAiming; }

    #endregion

    #region Make Singleton
    private static CameraMovement instance;
    public static CameraMovement GetInstance() { return instance; }
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDestroy()
    {
        if(instance != null)
            Destroy(instance);
    }

    #endregion

    private void Start()
    {
        cameraTransform = this.gameObject.GetComponentInChildren<Transform>();
        leftSideMovingLimit = Vector3.zero;
        rightSideMovingLimit = Vector3.zero;
    }

    private void Update()
    {
        CameraAimingState();
        
        if(!m_isChangingState)
        {
            if(!m_isAiming)
                ZoomInAndOut();
            CameraRotate();
        }
    }

    void CameraAimingState()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_isAiming = !m_isAiming;

            if (!m_isChangingState)
            {
                m_isChangingState = true;

                if (m_isAiming)
                {
                    m_saveEulerAngle = m_eulerAngle;
                    m_saveCameraTrans = Camera.main.transform;
                }
                else
                {
                    m_eulerAngle = m_saveEulerAngle;
                }
            }
        }

        if (!m_isAiming)
        {
            transform.position = Vector3.Lerp(transform.position, playerAxis.position, m_followingSpeed * Time.deltaTime);

            if (m_isChangingState)
            {
                ZoomOutToRegular();
            }
        }
        else
        {
            GetTarget();//HErC's addition

            if (m_isChangingState)
            {
                transform.position = Vector3.Lerp(transform.position, aimingAxis.position, m_followingSpeed * Time.deltaTime);
                ZoomInForAiming();
            }
            else
            {
                Vector3 temp = new Vector3(transform.position.x, 0, transform.position.z);
                if (Input.GetKey(KeyCode.A) && Vector3.Distance(leftSideMovingLimit, temp) > 0.25f)
                {
                    transform.Translate(Vector3.left * Time.deltaTime * m_sideMovingSpeed, Space.Self); //LEFT
                }
                if (Input.GetKey(KeyCode.D) && Vector3.Distance(rightSideMovingLimit, temp) > 0.25f)
                {
                    transform.Translate(Vector3.right * Time.deltaTime * m_sideMovingSpeed, Space.Self); //RIGHT
                }

            }
        }
    }

    void ZoomInAndOut() //this is just a regular zoom in and out using mouse scroll wheel.
    {
        m_distance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * m_zoomSpeed;

        Vector3 back = transform.forward * -1f;
        m_distance = Mathf.Clamp(m_distance, m_minDistance, m_maxDistance);

        RaycastHit hit;
        Debug.DrawRay(transform.position, back * m_distance, Color.red);
        if (Physics.Raycast(transform.position, back, out hit, m_distance))
        {
            if (hit.transform.tag != "Player")
                back *= hit.distance - 0.1f;
            else
                back *= m_distance;
        }
        else
            back *= m_distance;

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position + back, Time.deltaTime * 13f);
    }

    void CameraRotate()
    {
        if (m_isAiming) return;

        m_mouseX = Input.GetAxis("Mouse X");
        m_mouseY = Input.GetAxis("Mouse Y");

        if (Input.GetMouseButton(1)) //make rotation only when right mouse button is being pressed.
        {
            m_eulerAngle.y += m_mouseX * Time.deltaTime * m_rotSpeed;
            m_eulerAngle.x -= m_mouseY * Time.deltaTime * m_rotSpeed;
            m_eulerAngle.x = Mathf.Clamp(m_eulerAngle.x, m_minHeight, m_maxHeight);
        }

        Quaternion rotation = Quaternion.Euler(m_eulerAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, m_rotSpeed * Time.deltaTime);
    }

    void ZoomInForAiming()
    {
        m_eulerAngle.x = aimingAxis.rotation.x;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, aimingAxis.position, Time.deltaTime * m_aimingZoomSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, aimingAxis.rotation, Time.deltaTime * m_aimingZoomRotSpeed);
        if (Vector3.Distance(Camera.main.transform.position, aimingAxis.position) < 0.05f)
        {
            //Set Side moving range limit;
            leftSideMovingLimit = transform.position;
            rightSideMovingLimit = transform.position;
            Vector3 temp = Vector3.Cross(transform.forward, Vector3.up).normalized;
            leftSideMovingLimit += new Vector3(temp.x * m_leftMovingRange, temp.y, temp.z * m_leftMovingRange);
            rightSideMovingLimit -= new Vector3(temp.x * m_rightMovingRange, temp.y, temp.z * m_rightMovingRange);
            leftSideMovingLimit.y = 0;
            rightSideMovingLimit.y = 0;

            m_isChangingState = false;
        }
    }

    void ZoomOutToRegular()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, m_saveCameraTrans.position, Time.deltaTime * m_aimingZoomSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, m_saveCameraTrans.rotation, Time.deltaTime * m_aimingZoomRotSpeed);
        if (Vector3.Distance(Camera.main.transform.position, m_saveCameraTrans.position) < 0.05f)
            m_isChangingState = false;
    }


    //HErC'S ADDITIONS
    /// <summary>
    /// function designed to fire raycast and return a transform if there's a target, null otherwise
    /// This is set to the "ThunderPhysics" layer
    /// </summary>
    /// <returns></returns>
    public Transform GetTarget()
    {

        //ThunderPhysics' layer is 8
        //GravityPhysics' layer is 9
        int layers = ((1 << 8) + (1 << 9));
        RaycastHit tempTarget;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.transform.forward,
                           out tempTarget, raycastDistance, layers, QueryTriggerInteraction.Ignore))
        {
            return tempTarget.transform;
        }
        return null;
    }

    /// <summary>
    /// This function is supposed to return a placeholder transform for the gravity control scheme
    /// </summary>
    /// <returns></returns>
    public Transform GravityTarget()
    {

        if (m_isAiming)
        {
            gravityTarget.transform.position =
                this.gameObject.transform.position +
                (this.gameObject.transform.forward * raycastDistance * raycastFactor);
            gravityTarget.transform.rotation = this.gameObject.transform.rotation;
        }

        return gravityTarget.transform;
    }
    //END OF HErC'S ADDITIONS
}
