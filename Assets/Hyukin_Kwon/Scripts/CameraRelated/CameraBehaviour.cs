/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* CameraBehaviour
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 29 Feb 2020 - Hyukin
* 
* Inherits from Monobehaviour
*
* new Camera behaviour for the controller
* 
* Feb 9 mod: Re-added GetTarget() functionality to indicate
*            whether the targeted objects are chargeable
*            
* Feb 24, 25 mod: Modified GetTarget() functionality to include 
*                 shootable(destructible) objects
*                 Included getters and debug drawing of distances
*                 for chargeable, floatable and destructible objects
* *******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    // ** CamPivot --> Set position of z to -m_fDistance manually in the editor **...
    private GameObject CamPivot;
    private GameObject CamZoomPivot;
    private GameObject CamZoomtargetView;
    private GameObject player;
    private PlayerMovement playerMovementCs;
    public float m_fDistanceOrigin;
    public float m_fZoomDisOrigin;
    [SerializeField] float m_fDistance; //distance limit when player moving forward
    [SerializeField] float m_fCamRotateSpeed;
    [SerializeField] bool m_bCamRotateDirOnX; //use this to flip the roation direction;

    public float heightFromPlayer = 2.5f;
    private float heightFromPlayerOrigin;

    private Vector2 playerMoveAxis = Vector2.zero;
    private Vector2 camRotAxis = Vector2.zero;

    [SerializeField] float m_fCamMoveToPlayerBackSpeed; //speed of camera move to player's back when player is not moving;

    private bool m_bIsReseting = false;
    private bool m_bIsZooming = false;
    private bool m_bISZoomingBack = false;
    [SerializeField] float m_fZoomDis = 0.0f;
    [SerializeField] float m_fMaxZoomDis = 4.0f;
    [SerializeField] float m_fZoomSpeed;
    public bool m_bAdjustingDistance = false;
    [SerializeField] float m_fCamZoomSpeed; // speed of camera move to player's back when zoomed in or off.
    private float fdt;
    //Added by HErC:
    [SerializeField] private float m_fElectricDistance;// Max distance for chargeable finding
    [SerializeField] private float m_fGravityDistance; // Max distance for gravity detection
    [SerializeField] private float m_fLaserDistance;   // Max distance for laser cast

    #region Camera input Controls
    CameraInputAction inputAction;
    public bool zoomInput;
    bool resetInput;
    Vector2 rotateInput;
    GameObject transparencyObj;
    #endregion

    #region SetterAndGetter
    public bool GetIsZooming() { return m_bIsZooming; }
    public float GetDistance() { return m_fDistance; }
    //Added by HErC:
    public float GetElectricDistance() { return m_fElectricDistance; }
    public float GetGravityDistance() { return m_fGravityDistance; }
    public float GetLaserDistance() { return m_fLaserDistance; }
    #endregion
    #region Make Singleton
    private static CameraBehaviour instance;
    public static CameraBehaviour GetInstance() { return instance; }


    private void OnDestroy()
    {
        if (instance != null)
            Destroy(instance);
    }

    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;

        inputAction = new CameraInputAction();
        inputAction.CameraControls.Zoom.performed += ctx => zoomInput = true;
        inputAction.CameraControls.Zoom.canceled += ctx => zoomInput = false;
        inputAction.CameraControls.Reset.performed += ctx => resetInput = true;
        inputAction.CameraControls.Reset.canceled += ctx => resetInput = false;
        inputAction.CameraControls.Rotate.performed += ctx => rotateInput = ctx.ReadValue<Vector2>();
    }

    private void Start()
    {
        CamPivot = GameObject.Find("CamPivot");
        CamZoomPivot = GameObject.Find("CamZoomPivot");
        player = GameObject.FindGameObjectWithTag("Player");
        CamZoomtargetView = GameObject.Find("CamZoomtargetView");
        playerMovementCs = player.GetComponent<PlayerMovement>();
        heightFromPlayerOrigin = heightFromPlayer;
        m_fDistanceOrigin = m_fDistance;
        m_fZoomDisOrigin = Mathf.Abs(CamZoomPivot.transform.localPosition.z);
    }

    private void Update()
    {
        ZoomInMode();
    }


    private void FixedUpdate()
    {
        Camera.main.transform.position = transform.position;
        playerMoveAxis.x = playerMovementCs.movementInput.x;
        playerMoveAxis.y = playerMovementCs.movementInput.y;
        camRotAxis.x = rotateInput.x;
        camRotAxis.y = rotateInput.y;
        fdt = Time.fixedDeltaTime;

        if (!m_bIsZooming)
        {
            Rotate();
            Reset();
        }
        else
        {
            ZoomZoom();
        }
    }

    private void Rotate()
    {
        RaycastHit hit;
        float maxHeight = heightFromPlayerOrigin * 2.5f;
        if (rotateInput != Vector2.zero)
        {
            if (playerMoveAxis.x == 0)
            {
                if(m_bCamRotateDirOnX)
                    m_fCamRotateSpeed = (m_bCamRotateDirOnX) ? Mathf.Abs(m_fCamRotateSpeed) : -Mathf.Abs(m_fCamRotateSpeed);
                else
                    m_fCamRotateSpeed = (m_bCamRotateDirOnX) ? -Mathf.Abs(m_fCamRotateSpeed) : Mathf.Abs(m_fCamRotateSpeed);

                if (rotateInput.x > 0)
                {
                    transform.RotateAround(player.transform.position, Vector3.up, m_fCamRotateSpeed * fdt);
                }
                else if (rotateInput.x < 0)
                {
                    transform.RotateAround(player.transform.position, Vector3.up, -m_fCamRotateSpeed * fdt);
                }
            }
            if (m_bCamRotateDirOnX)
            {
                if (rotateInput.y < -0.2f && heightFromPlayer >= -playerMovementCs.GetDisToGround() + 0.5f)
                    heightFromPlayer -= fdt * m_fCamRotateSpeed / 20;
                else if (rotateInput.y > 0.2f && heightFromPlayer <= maxHeight)
                    heightFromPlayer += fdt * m_fCamRotateSpeed / 20;            
            }
            else if (!m_bCamRotateDirOnX)
            {
                if (rotateInput.y < -0.2f && heightFromPlayer <= maxHeight) 
                    heightFromPlayer += fdt * m_fCamRotateSpeed / 20;
                else if (rotateInput.y > 0.2f && heightFromPlayer >= -playerMovementCs.GetDisToGround() + 0.5f)
                    heightFromPlayer -= fdt * m_fCamRotateSpeed / 20;
            }
            if (Physics.Raycast(player.transform.position, Vector3.up, out hit, maxHeight))
            {
                if (heightFromPlayer < hit.distance)
                    heightFromPlayer += fdt * m_fCamRotateSpeed / 20;
            }
        }

        if ((heightFromPlayer < -playerMovementCs.GetDisToGround() + 0.5f))
            heightFromPlayer += fdt * m_fCamRotateSpeed / 20;
        else if (Physics.Raycast(player.transform.position, Vector3.up, out hit, maxHeight) ||
            Physics.Raycast(new Vector3(transform.position.x, player.transform.position.y, transform.position.z), Vector3.up, out hit, heightFromPlayerOrigin * 2))
            if (heightFromPlayer > hit.distance)
                heightFromPlayer -= fdt * m_fCamRotateSpeed / 20;

        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(transform.position.x, CamPivot.transform.position.y + heightFromPlayer, transform.position.z),
            playerMovementCs.GetMoveSpeed() * fdt);
      
        Vector3 targetPostition = new Vector3(player.transform.position.x,
                                        transform.position.y,
                                        player.transform.position.z);
        transform.rotation = new Quaternion();
        Camera.main.transform.rotation = new Quaternion();
        transform.LookAt(targetPostition);
        Camera.main.transform.rotation = transform.rotation;
        Quaternion q = Quaternion.LookRotation(player.transform.position - transform.position);
        Camera.main.transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 10000 * fdt);   
    }

    private void Reset()
    {
        if ((playerMoveAxis.x == 0 && playerMoveAxis.y == 0 && resetInput))
        {
            m_bIsReseting = true;
            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.back), out hit, m_fDistanceOrigin))
            {
                CamPivot.transform.localPosition = new Vector3(CamPivot.transform.localPosition.x, CamPivot.transform.localPosition.y, -hit.distance);
            }
            else
            {
                CamPivot.transform.localPosition = new Vector3(CamPivot.transform.localPosition.x, CamPivot.transform.localPosition.y, -m_fDistanceOrigin);
            }
        }
          
        if (m_bIsReseting)
        {
            Vector3 target = new Vector3(CamPivot.transform.position.x,
               CamPivot.transform.position.y + heightFromPlayer,
               CamPivot.transform.position.z);

            if (Vector3.Distance(transform.position, target) <= 0.25f)
            {
                m_bIsReseting = false;
            }
            transform.position = Vector3.MoveTowards(transform.position, target, m_fCamMoveToPlayerBackSpeed * fdt);
        }
    }

    private void ZoomInMode()
    {
        if (zoomInput) //step 1: key pressed
        {
            m_bIsZooming = true;
            RaycastHit hit;
            //step 2: shoot raycast to check if there is any object between zoom pivot point and player -> if there is make zoom pivot point closer to player
            if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.back), out hit, m_fZoomDisOrigin)) 
            {
                CamZoomPivot.transform.localPosition = new Vector3(CamZoomPivot.transform.localPosition.x, CamZoomPivot.transform.localPosition.y, -hit.distance);
            }
            else
            {
                CamZoomPivot.transform.localPosition = new Vector3(CamZoomPivot.transform.localPosition.x, CamZoomPivot.transform.localPosition.y, -m_fZoomDisOrigin);
            }

            //step 3: move to zoom point
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(CamZoomPivot.transform.position.x,
                CamZoomPivot.transform.position.y,
                CamZoomPivot.transform.position.z + m_fZoomDis),
                m_fCamZoomSpeed * Time.deltaTime);

            transform.LookAt(CamZoomtargetView.transform);
            Camera.main.transform.LookAt(CamZoomtargetView.transform);
        }
        else if (!zoomInput && m_bIsZooming) //step 4: stop zooming
        {
            m_bISZoomingBack = true;
        }

        if (m_bISZoomingBack) // step 5: move to Cam pivot point;
        {
            m_fZoomDis = 0.0f;
            Vector3 target = new Vector3(CamPivot.transform.position.x,
                CamPivot.transform.position.y + heightFromPlayer,
                CamPivot.transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, target,
                m_fCamZoomSpeed * Time.deltaTime);

            StartCoroutine(SetOfReset());
        }
    }

    IEnumerator SetOfReset()
    {
        yield return new WaitForSeconds(1f);
        m_bISZoomingBack = false;
        m_bIsZooming = false;
    }

    private void ZoomZoom() //lol
    {
        if(camRotAxis.y > 0 && m_fZoomDis < m_fMaxZoomDis)
        {
            m_fZoomDis += m_fZoomSpeed * fdt;
        }
        else if(camRotAxis.y < 0 && m_fZoomDis > 0)
        {
            m_fZoomDis -= m_fZoomSpeed * fdt;
        }
        m_fZoomDis = Mathf.Clamp(m_fZoomDis, 0, m_fMaxZoomDis);
    }

    /// <summary>
    /// Re-addition by HErC:
    /// Get Target function: Raycasts from the camera to the target at the center of the viewport.
    /// This function is aimed at indicating whether the target can be charged, moved or shot
    /// It works with Physics Layers!!!
    /// It DOES have some leftover implementation for the Gravity Gun
    /// </summary>
    /// <returns></returns>
    public Transform GetTarget() {

        Transform camTransform = Camera.main.transform;
        int thunderLayer = 1 << 8;
        int gravityLayer = 1 << 9; //maintained for gravity
        int laserLayer = 1 << 12;
        RaycastHit tempTarget;

        if (m_bIsZooming) {
            //Debug lines. Should be erased for final version
            Debug.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + this.gameObject.transform.forward * m_fLaserDistance, Color.red);
            Debug.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + this.gameObject.transform.forward * m_fGravityDistance, Color.magenta);
            Debug.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + this.gameObject.transform.forward * m_fElectricDistance, Color.green);

            if (Physics.Raycast(camTransform.position, camTransform.forward, out tempTarget, m_fElectricDistance, thunderLayer, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(camTransform.position, camTransform.forward, out tempTarget, m_fGravityDistance, gravityLayer, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(camTransform.position, camTransform.forward, out tempTarget, m_fLaserDistance, laserLayer, QueryTriggerInteraction.Ignore)) {
                return tempTarget.transform;
            }
        }
        return null;
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


#region old functions
//old variables
/*
    [SerializeField] float m_fMinDistance; //distance limit when player moving backward
    private float m_fMoveBackTimer = 2.0f;
    private float m_fCurMoveBackTimer = 0.0f;
    private bool m_bIsMoveBackTimerOn = false;
    private Vector3 m_targetDir;
    private bool m_bIsMovingToPlayerBack = false;

//old funcitons

    private void DistanceFixer()
    {
        m_fCurDistance = Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
        float disToCamPivot = Vector3.Distance(new Vector3(CamPivot.transform.position.x, 0, CamPivot.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
    
        if (m_fCurDistance >= m_fDistance * 2.5f && !m_bAdjustingDistance)
        {
            m_bAdjustingDistance = true;
        }
        if (m_bAdjustingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(CamPivot.transform.position.x, CamPivot.transform.position.y + heightFromPlayer, CamPivot.transform.position.z),
            disToCamPivot * 2.5f * fdt);
            if (m_fCurDistance < m_fDistance * 2f)
            {
                m_bAdjustingDistance = false;
            }
        }
    }
    
    
    private void MoveToPlayer()
    {
        float disToCamPivot = Vector3.Distance(new Vector3(CamPivot.transform.position.x, 0, CamPivot.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
        if (playerMoveAxis.y != 0)
        {
            if (m_fCurDistance > m_fDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(CamPivot.transform.position.x, CamPivot.transform.position.y + heightFromPlayer, CamPivot.transform.position.z),
                    disToCamPivot * 2.5f * fdt);
            }
            else if (m_fCurDistance < m_fMinDistance && m_fCurDistance > 0)
            {
                Vector3 tempSpeed = player.GetComponent<PlayerMovement>().GetOverallSpeed() * player.GetComponent<PlayerMovement>().GetMoveSpeed();
                tempSpeed.x /= 16;
                RaycastHit hit;
                if (!Physics.Raycast(transform.localPosition, Vector3.back, out hit, 1))
                {
                    transform.Translate(Vector3.back * tempSpeed.magnitude * fdt);
                }
                else
                {
                    transform.Translate(Vector3.forward * tempSpeed.magnitude * fdt);
                }
            }
        }
    }
    
    private void MoveToPlayerBack() //Move to back of character when player is moving for certain amount of time 
    {
        if (m_bIsMoveBackTimerOn)
        {
            heightFromPlayer = heightFromPlayerOrigin;
            Vector3 target = new Vector3(CamPivot.transform.position.x,
                CamPivot.transform.position.y + heightFromPlayer,
                CamPivot.transform.position.z);
    
            if (Vector3.Distance(transform.position, target) <= 0.25f)
            {
                m_bIsMoveBackTimerOn = false;
            }
    
            transform.position = Vector3.MoveTowards(transform.position, target, m_fCamMoveToPlayerBackSpeed * fdt);
        }
    
        if (playerMoveAxis == Vector2.zero && rotateInput == Vector2.zero)
        {
            if (!m_bIsMoveBackTimerOn)
                m_fCurMoveBackTimer += fdt;
            if (m_fCurMoveBackTimer >= m_fMoveBackTimer)
            {
                m_bIsMoveBackTimerOn = true;
                m_fCurMoveBackTimer = 0.0f;
            }
    
            if (Input.GetKey(KeyCode.B) || Input.GetKey(KeyCode.Joystick1Button3))
            {
                if (!m_bIsMovingToPlayerBack)
                {
                    m_targetDir = player.transform.forward;
                }
                m_bIsMovingToPlayerBack = true;
            }
        }
        else
        {
            m_bIsMovingToPlayerBack = false;
        }
    
        if (m_bIsMovingToPlayerBack)
        {
            if (Vector3.Angle(m_targetDir, transform.forward) <= 1)
            {
                m_bIsMovingToPlayerBack = false;
                m_targetDir = player.transform.forward;
                Debug.Log("Stop Rotating Around!");
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(CamPivot.transform.position.x,
                CamPivot.transform.position.y + heightFromPlayer * 0.6f,
                CamPivot.transform.position.z),
                45 * Time.deltaTime);
    
            }
        }
    }
*/
#endregion