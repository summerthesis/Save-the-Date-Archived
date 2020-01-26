﻿/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* CameraBehaviour
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 26 Jan 2020 - Hyukin
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
    private GameObject PlayerCenter;
    private PlayerMovement playerMovementCs;
    [SerializeField] float m_fDistance; //distance limit when player moving forward
    [SerializeField] float m_fMinDistance; //distance limit when player moving backward

    private float heightFromPlayer = 2.5f;

    private float m_fVertical;
    private float m_fHorizontal;
    private float m_fLastHOrizontal = 0.0f; //this will store negative or positive number from m_fHorizontal.
    private bool m_bMoveToBackOn = false;
    private float m_fMoveToBackTime = 2.0f;
    private float m_fCurMoveToBackTime = 0.0f;

    private bool m_bIsZooming = false;

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
        PlayerCenter = GameObject.Find("PlayerCenter");
        playerMovementCs = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        ZoomInMode();
    }

    private void FixedUpdate()
    {
        Camera.main.transform.position = transform.position;

        PlayerCenter.transform.LookAt(new Vector3(transform.position.x, PlayerCenter.transform.position.y, transform.position.z));

        if (!m_bIsZooming)
        {
            MoveToBackWhenNotMoving();
            MoveToPlayer();
            Rotate();
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
        Camera.main.transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 10000 * Time.deltaTime);
    }

    private void MoveToPlayer()
    {
        float distance = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z).magnitude;
        //Debug.Log(distance);
        float dt = Time.fixedDeltaTime;
        Debug.DrawRay(player.transform.position, -player.transform.forward * m_fDistance, Color.red);
        Debug.DrawRay(player.transform.position, (transform.position - player.transform.position), Color.green);

        if (distance > m_fDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(CamPivot.transform.position.x, CamPivot.transform.position.y + heightFromPlayer, CamPivot.transform.position.z),
                playerMovementCs.GetMoveSpeed() * dt);
        }
        else if(distance < m_fMinDistance && distance > 0)
        {
            transform.Translate(-Vector3.forward * player.GetComponent<PlayerMovement>().GetMoveSpeed() * dt);
        }
    }

    private void MoveToBackWhenNotMoving() //Move to back of character when player is moving for certain amount of time 
    {
        float dt = Time.deltaTime;
        m_fHorizontal = Input.GetAxis("Horizontal");
        m_fVertical = Input.GetAxis("Vertical");

        if (!m_bMoveToBackOn)
        {
            if(m_fHorizontal != 0)
                m_fLastHOrizontal = m_fHorizontal;

            if (m_fHorizontal == 0 && m_fVertical == 0)
            {
                m_fCurMoveToBackTime += dt;
                if (m_fCurMoveToBackTime >= m_fMoveToBackTime)
                {
                    m_fCurMoveToBackTime = 0.0f;
                    m_bMoveToBackOn = true;
                }
            }
            else if (m_fHorizontal != 0 || m_fVertical != 0)
            {
                m_fCurMoveToBackTime = 0.0f;
                m_bMoveToBackOn = false;
            }
        }
        else if(m_bMoveToBackOn)
        {
            //Debug.Log("Player RotY: " + player.transform.rotation.y + ", Cam RotY: " + transform.rotation.y);
            //if (player.transform.rotation.y != transform.rotation.y)
            //{
            //    if(m_fLastHOrizontal < 0)
            //        transform.RotateAround(player.transform.position, Vector3.up, -100 * dt);
            //    else if (m_fLastHOrizontal > 0)
            //        transform.RotateAround(player.transform.position, Vector3.up, 100 * dt);
            //}
            //else
            //{
            //    m_fLastHOrizontal = 0;
            //}
            float distance = new Vector2(CamPivot.transform.position.x - transform.position.x, CamPivot.transform.position.z - transform.position.z).magnitude;
            if(distance <= 0.2f)
            {
                transform.position = new Vector3(CamPivot.transform.position.x, CamPivot.transform.position.y + heightFromPlayer, CamPivot.transform.position.z);
                m_fCurMoveToBackTime = 0.0f;
                m_bMoveToBackOn = false;
            }
        }
    }

    private void ZoomInMode()
    {
        if(Input.GetKey(KeyCode.Joystick1Button0))
        {
            m_bIsZooming = true;

            transform.position = Vector3.MoveTowards(transform.position,
               new Vector3(CamZoomPivot.transform.position.x, 
               CamZoomPivot.transform.position.y + heightFromPlayer * 0.6f,
               CamZoomPivot.transform.position.z),
               playerMovementCs.GetMoveSpeed() * 10.0f * Time.deltaTime);

            transform.LookAt(player.transform);
            Camera.main.transform.LookAt(player.transform);
        }
        else if(!Input.GetKey(KeyCode.Joystick1Button0))
        {
            m_bIsZooming = false;
        }
    }

}
