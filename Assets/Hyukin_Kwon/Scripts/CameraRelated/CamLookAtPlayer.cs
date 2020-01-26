/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* CamLookAtPlayer
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 25 Jan 2020 - Hyukin
* 
* Inherits from Monobehaviour
* *******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLookAtPlayer : MonoBehaviour
{
    private GameObject player;
    private GameObject CameraBody;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CameraBody = GameObject.Find("CameraBody");
    }

    void Update()
    {
        transform.position = CameraBody.transform.position;

        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.x = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 500 * Time.fixedDeltaTime);
    }
}
