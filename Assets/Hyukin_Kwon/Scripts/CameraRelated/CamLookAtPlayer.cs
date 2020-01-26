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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Quaternion q = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, new Quaternion(q.x, transform.rotation.y, transform.rotation.z, 1), 500 * Time.deltaTime);
    }
}
