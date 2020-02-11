/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* CameraBehaviour
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 04 Feb 2020 - Hyukin
* 
* Inherits from Monobehaviour
*
* new Camera behaviour for the controller
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
    [SerializeField] float m_fDistance; //distance limit when player moving forward
    [SerializeField] float m_fMinDistance; //distance limit when player moving backward

    private float heightFromPlayer = 2.5f;

    private float m_fVertical;
    private float m_fHorizontal;
    private float m_fLastHOrizontal = 0.0f; //this will store negative or positive number from m_fHorizontal.
    private float m_fLastVertical = 0.0f;

    private bool m_bIsZooming = false;
    private bool m_bISZoomingBack = false;
    private bool m_bIsMovingToPlayerBack = false;
    private Vector3 m_targetDir;
    private float fdt;

    public bool GetIsZooming() { return m_bIsZooming; }
    public float GetDistance() { return m_fDistance; }
    public float GetMinDistance() { return m_fMinDistance; }

    #region Make Singleton
    private static CameraBehaviour instance;
    public static CameraBehaviour GetInstance() { return instance; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDestroy()
    {
        if (instance != null)
            Destroy(instance);
    }

    #endregion

    private void Start()
    {
        CamPivot = GameObject.Find("CamPivot");
        CamZoomPivot = GameObject.Find("CamZoomPivot");
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovementCs = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        ZoomInMode();
    }

    private void FixedUpdate()
    {
        Camera.main.transform.position = transform.position;
        m_fHorizontal = Input.GetAxis("Horizontal");
        m_fVertical = Input.GetAxis("Vertical");
        fdt = Time.fixedDeltaTime;
        if (!m_bIsZooming)
        {
            MoveToPlayer();
            Rotate();
            MoveToPlayerBack();
        }

    }

    private void Rotate()
    {
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
        float distance = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z).magnitude;
        Debug.DrawRay(player.transform.position, (transform.position - player.transform.position), Color.green);

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
            Debug.Log(tempSpeed.magnitude);
            transform.Translate(-Vector3.forward * tempSpeed.magnitude * fdt);
        }
    }

    private void MoveToPlayerBack() //Move to back of character when player is moving for certain amount of time 
    {

        if (m_fHorizontal == 0 && m_fVertical == 0)
        {
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
                m_fLastHOrizontal = 0;
                m_fLastVertical = 0;
                m_bIsMovingToPlayerBack = false;
                m_targetDir = player.transform.forward;
                Debug.Log("Stop Rotating Around!");
            }
            else
            {
                //transform.RotateAround(player.transform.position, Vector3.up, 300 * fdt);
                transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(CamPivot.transform.position.x,
                CamPivot.transform.position.y + heightFromPlayer * 0.6f,
                CamPivot.transform.position.z),
                45 * Time.deltaTime);

            }
        }
    }

    private void ZoomInMode()
    {
        if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Z))
        {
            m_bIsZooming = true;

            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(CamZoomPivot.transform.position.x,
                CamZoomPivot.transform.position.y + heightFromPlayer * 0.6f,
                CamZoomPivot.transform.position.z),
                playerMovementCs.GetMoveSpeed() * 10.0f * Time.deltaTime);

            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            Camera.main.transform.LookAt(player.transform);
        }
        else if (!Input.GetKey(KeyCode.Joystick1Button0) && m_bIsZooming)
        {
            m_bISZoomingBack = true;
        }

        if(m_bISZoomingBack)
        {
            Vector3 target = new Vector3(CamPivot.transform.position.x,
                CamPivot.transform.position.y + heightFromPlayer,
                CamPivot.transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, target,
                playerMovementCs.GetMoveSpeed() * 10.0f * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) <= 0.25f)
            {
                m_bISZoomingBack = false;
                m_bIsZooming = false;
            }
        }
    }

}
