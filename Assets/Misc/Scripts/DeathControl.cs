using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathControl : MonoBehaviour
{
    public bool isDead = false;
    public float deathDepth = -20;
    public float deathTime = 2;
    public float offset = 2;
    private float i;
    public Transform lastLocation;
    public GameObject checkpointBlock;
    private Quaternion  lastRotation;
    // Start is called before the first frame update
    void Start()
    {
        i = deathTime;
        checkpointBlock = GameObject.Find("StartBlock");

        lastLocation = checkpointBlock.GetComponent<checkpointScript>().checkpoint;
    }

    // Update is called once per frame
    void Update()
    {
        lastLocation = checkpointBlock.GetComponent<checkpointScript>().checkpoint;
        if (transform.position.y < deathDepth)
        {
            isDead = true;
        }
        if (isDead)
        {
            i -= Time.deltaTime;
            if (i < 0)
            {
                Restart();
            }
        }

    }
    public void Restart()
    {
        transform.position = new Vector3(lastLocation.position.x, lastLocation.position.y + offset, lastLocation.position.z);
        transform.rotation = lastLocation.rotation;
        i = deathTime;
        isDead = false;
    }
}
