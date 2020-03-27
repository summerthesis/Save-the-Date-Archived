using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPlate : MonoBehaviour
{
    public GameObject mHookReceiver;
    public GameObject mPlayer;
    private Collider mPCollider;
    public bool isActive;
    public bool isVisible; 
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GameObject.Find("Character");
        mPCollider = mPlayer.GetComponent<Collider>();
        rend = mHookReceiver.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        
        if (col == mPCollider)
        {
            // It is object B
            Debug.Log("Player entered hook zone");
            isActive = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col == mPCollider)
        {
            // It is object B
            Debug.Log("Player Left hook zone");
            isVisible = false;
            isActive = false;
        }
    }
    
 

}

