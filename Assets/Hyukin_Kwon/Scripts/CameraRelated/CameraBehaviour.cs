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
    [SerializeField] float m_fDistance;
    [SerializeField] float m_fMoveSpeed;
    [SerializeField] float m_fRotSpeed;

    private float m_fMaxDisMulti = 2.0f;
    private float m_fMoveSpeedMulti = 3.0f;


    private float m_fHorizontal;
    private float m_fVertical;

    public float GetHorizontal() { return m_fHorizontal; }
    public float GetVertical() { return m_fVertical; }
    public bool isIdle() { return (m_fHorizontal == 0 && m_fVertical == 0) ? true : false; }

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
    }

    private void Update()
    {
        m_fHorizontal = Input.GetAxis("Horizontal");
        m_fVertical = Input.GetAxis("Vertical");
        Rotate();
        Move();
    }

    private void Rotate()
    {
        if(m_fHorizontal > 0.1f)
        {
            Debug.Log("Rot to right");
            transform.Rotate(Vector3.up * m_fRotSpeed * Time.deltaTime, Space.World);
        }
        else if(m_fHorizontal < -0.1f)
        {
            Debug.Log("Rot to left");
            transform.Rotate(Vector3.up * -m_fRotSpeed * Time.deltaTime, Space.World);
        }
    }

    private void Move()
    {
        if (m_fVertical > 0.2f)
        {
            transform.Translate(Vector3.forward * m_fMoveSpeed * Time.deltaTime, Space.Self);
        }
        else if(m_fVertical < -0.2f)
        {
            transform.Translate(-Vector3.forward * m_fMoveSpeed * Time.deltaTime, Space.Self);
        }
    }

}
