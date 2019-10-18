using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBlock : MonoBehaviour
{
    public checkpointScript myScript;
    public GameObject player;
    private bool isCheck = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.GetComponent<DeathControl>().checkpointBlock = gameObject;

            if (!isCheck)
            {
                AudioSource myAudio = GetComponent<AudioSource>();
                myAudio.Play();
                isCheck = true;
            }
        }
    }
}
