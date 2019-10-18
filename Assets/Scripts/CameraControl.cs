using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float speed = 3;
    private float rotateX;
    private float rotateY;
    private Vector3 currentPosition;
    private Vector3 startPosition;
    private bool deathToggle = false;
    GameObject player;
    DeathControl deathControl;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        deathControl = player.GetComponent<DeathControl>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * 1, 0, 0));
        if (!deathControl.isDead)
        {
            currentPosition = transform.position;
            if (deathToggle)
            {
                deathToggle = false;
                transform.position = new Vector3(player.transform.position.x + 0f, player.transform.position.y + 2.5f, player.transform.position.z -6.8f); //The offsets should be variables but it's not working and I'm not sure why. RIP.
                transform.LookAt(player.transform);
            }
        }
        else
        {
            deathToggle = true;
            transform.position = currentPosition;
            transform.LookAt(player.transform);
        }
    }
}
