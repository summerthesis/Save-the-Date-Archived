/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* PlayerMovement
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 25 Jan 2020 - Hyukin
* 
* Inherits from Monobehaviour
*
* new Player behaviour
* *******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject cameraTarget;
    [SerializeField] float m_fMoveSpeed;
    [SerializeField] float m_fRotSpeed;

    private void Start()
    {
        cameraTarget = GameObject.Find("CameraTarget");
        transform.position = cameraTarget.transform.position;
    }
    private void Update()
    {
       // MoveTo();
    }

    private void MoveTo()
    {
        if(CameraBehaviour.GetInstance().isIdle())
        {
            Quaternion q1 = new Quaternion(transform.rotation.x, cameraTarget.transform.rotation.y, transform.rotation.z, 1);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q1, m_fRotSpeed * Time.deltaTime);
            return;
        }

        Quaternion q2 = Quaternion.LookRotation(cameraTarget.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q2, m_fRotSpeed * Time.deltaTime);
        //transform.LookAt(cameraTarget.transform);
        transform.position = Vector3.MoveTowards(transform.position, cameraTarget.transform.position, m_fMoveSpeed * Time.deltaTime);
    }



}
