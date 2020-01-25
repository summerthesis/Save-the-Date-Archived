/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* CameraLookAt
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 25 Jan 2020 - Hyukin
* 
* Inherits from Monobehaviour
*
* just a simple script that only to facing to player.
* *******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    //private GameObject player;
    private GameObject cameraTarget;

    [SerializeField] float m_fLookAtSpeed;

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        cameraTarget = GameObject.Find("CameraTarget");
    }

    private void Update()
    {
        RoateToTarget();
    }

    private void RoateToTarget()
    {
        //Quaternion q = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.LookAt(cameraTarget.transform);
    }
}
