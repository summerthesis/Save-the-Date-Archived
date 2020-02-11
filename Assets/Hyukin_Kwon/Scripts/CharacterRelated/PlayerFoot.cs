/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* PlayerFoot
* Modified: 25 Jan 2020 - Hyukin
* Last Modified: 11 Feb 2020 - Hyukin
* 
* Inherits from Monobehaviour
* *******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    [SerializeField] GameObject Player;
    PlayerMovement PlayerMovementCS;
    [SerializeField] float rayDis;

    private void Start()
    {
        PlayerMovementCS = Player.GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, rayDis))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * rayDis, Color.green);
            if (hit.transform.tag != "Player")
            {
                if(hit.transform.tag == "Stair")
                {
                    PlayerMovementCS.SetIsOnStair(true);
                }
                else if(hit.transform.tag != "Stair")
                {
                    PlayerMovementCS.SetIsOnStair(false);
                }
                PlayerMovementCS.SetIsGround(true);
            }

        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * rayDis, Color.red);
        }
    }
}
