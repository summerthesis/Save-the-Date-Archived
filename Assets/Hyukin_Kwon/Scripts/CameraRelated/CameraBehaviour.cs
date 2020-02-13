/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* CameraBehaviour
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 09 Feb 2020 - HErC
* 
* Inherits from Monobehaviour
*
* new Camera behaviour for the controller
* 
* Feb 9 mod: Re-added GetTarget() functionality to indicate
*            whether the targeted objects are chargeable
* *******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    // ** CamPivot --> Set position of z to -m_fDistance manually in the editor **...
    private GameObject CamPivot;
    private GameObject CamZoomPivot;
    private GameObject player;
    private PlayerMovement playerMovementCs;
    public float m_fDistanceOrigin;
    public float m_fZoomDisOrigin;
    [SerializeField] float m_fDistance; //distance limit when player moving forward
    [SerializeField] float m_fMinDistance; //distance limit when player moving backward
    [SerializeField] float m_fCamRotateSpeed;
    [SerializeField] bool m_bCamRotateDirOnX; //use this to flip the roation direction;

    public float heightFromPlayer = 2.5f;
    private float heightFromPlayerOrigin;

    private Vector2 playerMoveAxis = Vector2.zero;
    private Vector2 camRotAxis = Vector2.zero;

    private float m_fMoveBackTimer = 2.0f;
    private float m_fCurMoveBackTimer = 0.0f;
    private bool m_bIsMoveBackTimerOn = false;
    [SerializeField] float m_fCamMoveToPlayerBackSpeed; //speed of camera move to player's back when player is not moving;

    private bool m_bIsReseting = false;
    private bool m_bIsZooming = false;
    private bool m_bISZoomingBack = false;
    private bool m_bIsMovingToPlayerBack = false;
    [SerializeField] float m_fCamZoomSpeed; // speed of camera move to player's back when zoomed in or off.
    private Vector3 m_targetDir;
    private float fdt;
    //Added by HErC:
    [SerializeField] private float m_fRaycastDistance;//Max distance for target finding

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
    public float GetMinDistance() { return m_fMinDistance; }
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
            MoveToPlayer();
            Rotate();
            //MoveToPlayerBack(); // I don't really know if you need this function try to put it back and decide which looks better.
            Reset();
        }

    }

    private void Rotate()
    {
        RaycastHit hit;
        if (rotateInput != Vector2.zero)
        {
            if (playerMoveAxis.x == 0)
            {
                m_fCamRotateSpeed = (m_bCamRotateDirOnX) ? Mathf.Abs(m_fCamRotateSpeed) : -Mathf.Abs(m_fCamRotateSpeed);
                if (rotateInput.x > 0)
                {
                    transform.RotateAround(player.transform.position, Vector3.up, m_fCamRotateSpeed * fdt);
                }
                else if (rotateInput.x < 0)
                {
                    transform.RotateAround(player.transform.position, Vector3.up, -m_fCamRotateSpeed * fdt);
                }
            }
            if (rotateInput.x <= 0.2f && rotateInput.x >= -0.2f)
            {
                if (rotateInput.y < -0.2f && heightFromPlayer >= -playerMovementCs.GetDisToGround() + 0.5f)
                {
                    heightFromPlayer -= fdt * m_fCamRotateSpeed / 20;
                }
                else if (Physics.Raycast(player.transform.position, Vector3.up, out hit, heightFromPlayerOrigin * 3))
                {
                    if (heightFromPlayer < hit.distance)
                    {
                        heightFromPlayer += fdt * m_fCamRotateSpeed / 20;
                    }
                }
                else if (rotateInput.y > 0.2f && heightFromPlayer <= heightFromPlayerOrigin * 3)
                {
                    heightFromPlayer += fdt * m_fCamRotateSpeed / 20;
                }              
            }
        }
        if ((heightFromPlayer < -playerMovementCs.GetDisToGround() + 0.5f))
        {
            heightFromPlayer += fdt * m_fCamRotateSpeed / 20;
        }
             
        else if (Physics.Raycast(player.transform.position, Vector3.up, out hit, heightFromPlayerOrigin * 3) ||
            Physics.Raycast(new Vector3(transform.position.x, player.transform.position.y, transform.position.z), Vector3.up, out hit, heightFromPlayerOrigin * 2))
        {
            if (heightFromPlayer > hit.distance)
            {
                heightFromPlayer -= fdt * m_fCamRotateSpeed / 20;
            }
        }


        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(transform.position.x, CamPivot.transform.position.y + heightFromPlayer, transform.position.z),
            playerMovementCs.GetMoveSpeed() * fdt);

        transform.rotation = new Quaternion();
        Camera.main.transform.rotation = new Quaternion();
        Vector3 targetPostition = new Vector3(player.transform.position.x,
                                        transform.position.y,
                                        player.transform.position.z);
        transform.LookAt(targetPostition);
        Camera.main.transform.rotation = transform.rotation;
        Quaternion q = Quaternion.LookRotation(player.transform.position - transform.position);
        Camera.main.transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 10000 * fdt);
    }

    private void MoveToPlayer()
    {
        float distance = Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
        if(playerMoveAxis != Vector2.zero)
        {
            if (distance > m_fDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(CamPivot.transform.position.x, CamPivot.transform.position.y + heightFromPlayer, CamPivot.transform.position.z),
                    playerMovementCs.GetMoveSpeed() * fdt);
            }
            else if (distance < m_fMinDistance && distance > 0)
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
            Vector3 target = new Vector3(CamPivot.transform.position.x,
                CamPivot.transform.position.y + heightFromPlayer,
                CamPivot.transform.position.z);

            if (Vector3.Distance(transform.position, target) <= 0.25f)
            {
                m_bIsMoveBackTimerOn = false;
            }

            transform.position = Vector3.MoveTowards(transform.position, target, m_fCamMoveToPlayerBackSpeed * fdt);
        }

        if (playerMoveAxis.x == 0 && playerMoveAxis.y == 0)
        {
            if(!m_bIsMoveBackTimerOn)
                m_fCurMoveBackTimer += fdt;
            if(m_fCurMoveBackTimer >= m_fMoveBackTimer)
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
                CamZoomPivot.transform.position.y + heightFromPlayer * 0.6f,
                CamZoomPivot.transform.position.z),
                m_fCamZoomSpeed * Time.deltaTime);

            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            Camera.main.transform.LookAt(player.transform);
        }
        else if (!zoomInput && m_bIsZooming) //step 4: stop zooming
        {
            m_bISZoomingBack = true;
        }

        if (m_bISZoomingBack) // step 5: move to Cam pivot point;
        {
            Vector3 target = new Vector3(CamPivot.transform.position.x,
                CamPivot.transform.position.y + heightFromPlayer,
                CamPivot.transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, target,
                m_fCamZoomSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) <= 0.25f)
            {
                m_bISZoomingBack = false;
                m_bIsZooming = false;
            }
        }
    }

    private void SetTransparency()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, transform.position - player.transform.position, out hit, m_fDistanceOrigin))
        {
            if (hit.transform.tag != "Camera")
            {
                transparencyObj = hit.transform.gameObject;
                Debug.Log("hit something: " + hit.transform.gameObject);
                Color color = transparencyObj.GetComponent<MeshRenderer>().material.color;
                color.a = 0.5f;
                transparencyObj.GetComponent<MeshRenderer>().material.color = color;
            }
        }
        else
        {
            if (transparencyObj != null)
            {
                Color color = transparencyObj.GetComponent<MeshRenderer>().material.color;
                color.a = 1;
                transparencyObj.GetComponent<MeshRenderer>().material.color = color;
            }
        }
    }

    /// <summary>
    /// Re-addition by HErC:
    /// Get Target function: Raycasts from the camera to the target at the center of the viewport.
    /// This function is aimed at indicating whether the target can be charged or moved
    /// It works with Physics Layers!!!
    /// It DOES have some leftover implementation for the Gravity Gun
    /// </summary>
    /// <returns></returns>
    public Transform GetTarget() {

        Transform camTransform = Camera.main.transform;
        int thunderLayer = 1 << 8;
        int gravityLayer = 1 << 9; //maintained
        int layers = thunderLayer + gravityLayer;
        RaycastHit tempTarget;

        if (Physics.Raycast(camTransform.position, camTransform.forward, out tempTarget, m_fRaycastDistance, layers, QueryTriggerInteraction.Ignore)) {
            return tempTarget.transform;
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
