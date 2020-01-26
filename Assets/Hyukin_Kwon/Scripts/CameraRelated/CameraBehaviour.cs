/*********************************************************
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
    private GameObject CamPivot;
    private GameObject player;
    private PlayerMovement playerMovementCs;
    [SerializeField] float m_fDistance; //distance limit when player moving forward
    [SerializeField] float m_fMinDistance; //distance limit when player moving backward

    private float heightFromPlayer = 2.5f;

    private float m_fVertical;
    private float m_fHorizontal;
    private bool m_bMoveToBackOn = false;
    private float m_fMoveToBackTime = 2.0f;
    private float m_fCurMoveToBackTime = 0.0f;

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
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovementCs = player.GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        Camera.main.transform.position = transform.position;
        MoveToBackWhenNotMoving();
        MoveToPlayer();
        Rotate();
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
        float dt = Time.fixedDeltaTime;
        m_fHorizontal = Input.GetAxis("Horizontal");
        m_fVertical = Input.GetAxis("Vertical");

        if (!m_bMoveToBackOn)
        {
            if (m_fHorizontal == 0 && m_fVertical == 0)
            {
                m_fCurMoveToBackTime += dt;
                if (m_fCurMoveToBackTime >= m_fMoveToBackTime)
                {
                    m_fCurMoveToBackTime = 0.0f;
                    m_bMoveToBackOn = true;
                }
            }
            else
            {
                m_fCurMoveToBackTime = 0.0f;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,
               new Vector3(CamPivot.transform.position.x, CamPivot.transform.position.y + heightFromPlayer, CamPivot.transform.position.z),
               playerMovementCs.GetMoveSpeed() * dt);
            float distance = new Vector2(CamPivot.transform.position.x - transform.position.x, CamPivot.transform.position.z - transform.position.z).magnitude;
            if(distance <= 0.2f)
            {
                transform.position = new Vector3(CamPivot.transform.position.x, CamPivot.transform.position.y + heightFromPlayer, CamPivot.transform.position.z);
                m_fCurMoveToBackTime = 0.0f;
                m_bMoveToBackOn = false;
            }
        }

    }

}
