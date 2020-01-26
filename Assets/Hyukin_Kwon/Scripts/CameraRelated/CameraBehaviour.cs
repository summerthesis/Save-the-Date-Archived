/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* CameraBehaviour
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 25 Jan 2020 - Hyukin
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
    private GameObject player;
    private PlayerMovement playerMovementCs;
    [SerializeField] float m_fDistance; //distance limit when player moving forward
    [SerializeField] float m_fMinDistance; //distance limit when player moving backward
    [SerializeField] float m_fRotSpeed;
    [SerializeField] float m_fmoveSpeed;

    private float heightFromPlayer = 2.5f;

    private float m_fHorizontal;

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
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovementCs = player.GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        Rotate();
        MoveToPlayer();
    }

    private void Rotate()
    {

    }

    private void MoveToPlayer()
    {
        Vector2 vecDis = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z);
        float distance = vecDis.magnitude;
        float dt = Time.fixedDeltaTime;

        if(distance > m_fDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(player.transform.position.x, player.transform.position.y + heightFromPlayer, player.transform.position.z - m_fDistance),
                playerMovementCs.GetMoveSpeed() * dt);
        }
        else if(distance < m_fMinDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(player.transform.position.x, player.transform.position.y + heightFromPlayer, player.transform.position.z - m_fMinDistance),
                playerMovementCs.GetMoveSpeed() * dt);
        }
    }


}
