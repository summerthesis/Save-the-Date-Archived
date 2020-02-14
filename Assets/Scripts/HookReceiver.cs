using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookReceiver : MonoBehaviour
{
    public Material idleMaterial;
    public Material focusMaterial;
    public Material activeMaterial;

    public GameObject mPlayer;
    public GameObject[] mWayPoints;

    public int step;

    public float pDistance;
    
    public bool isInRange;

    enum States {Idle = 0, Focus = 1, Active = 2, Finished = 3 }
    public int State;
    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GameObject.Find("PlayerCharacter");

    }

    // Update is called once per frame
    void Update()
    {
        switch(State)
        
        {
            case 0:
                Debug.Log("Entered Idle State");
                
                break;
            
            case 1:
                Debug.Log("Entered Focussed State");

                break;
            
            case 2:
                
                
                break;
           
            case 3:
                
                
                break;
           
            default:
                break;
        }


    }
}
