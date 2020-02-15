using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookReceiver : MonoBehaviour
{
    PlayerInputAction controls;

    public Material idleMaterial;
    public Material focusMaterial;
    public Material activeMaterial;
    public GameObject mCamera;
    public GameObject mPlayer;
    public GameObject[] mWayPoints;
    public GameObject mPlate;
    public int step;
    public bool isReady;
    public bool isVisible;
    public bool testBool;
    enum States { Idle = 0, Focus = 1, Active = 2, Finished = 3 }
    public int State;
    public float speed = 0.1f;

    void Awake()
    {
        controls = new PlayerInputAction();
        controls.PlayerControls.ActivateHook.performed += ctx => Grapple();
        controls.PlayerControls.Enable();
    }

    void Start()
    {
        mPlayer = GameObject.Find("Character");
        mCamera = GameObject.Find("CameraBody");
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)

        {
            case 0:
                Debug.Log("Entered Idle State");



                if (mPlate.GetComponent<HookPlate>().isActive)
                {
                    if (IsVisibleToCamera(transform))
                    {
                        isVisible = true;
                        isReady = true;
                    }
                    else
                        isVisible = false;
                }
                else isReady = false;

                break;

            case 1:
                float step = speed * Time.deltaTime;
                if (Vector3.Distance(mWayPoints[0].transform.position, mPlayer.transform.position) < 0.001f)
                {
                    State++;
                }
                else
                {
                    if (speed < 1.0f) speed += 0.03f;
                    mPlayer.transform.position = Vector3.MoveTowards(mPlayer.transform.position, mWayPoints[0].transform.position, speed);
                }
                    
                break;

            case 2:
                if (Vector3.Distance(mWayPoints[1].transform.position, mPlayer.transform.position) < 0.001f)
                {
                    State++;
                }
                else
                    mPlayer.transform.position = Vector3.MoveTowards(mPlayer.transform.position, mWayPoints[1].transform.position, 0.25f);

                break;

            case 3:

                if (Vector3.Distance(mWayPoints[2].transform.position, mPlayer.transform.position) < 0.001f)
                {
                    State++;
                }
                else
                    mPlayer.transform.position = Vector3.MoveTowards(mPlayer.transform.position, mWayPoints[2].transform.position, 0.25f);
                break;
            case 4:
                if (Vector3.Distance(mWayPoints[3].transform.position, mPlayer.transform.position) < 0.001f)
                {
                    State++;
                }
                else
                    mPlayer.transform.position = Vector3.MoveTowards(mPlayer.transform.position, mWayPoints[3].transform.position, 0.25f);
                break;
             
            case 5: // Complete
                mPlayer.GetComponent<Rigidbody>().useGravity = true;
                speed = 0.1f;
                if (Vector3.Distance(mWayPoints[2].transform.position, mCamera.transform.position) < 11.501f)
                {
                    State=0;
                }
                else
                    mCamera.transform.position = Vector3.MoveTowards(mCamera.transform.position, mWayPoints[2].transform.position, 1.25f);
         
                Debug.Log("GRAPPLE COMPLETE");
                break;
            default:
                break;
        }


    }

    public static bool IsVisibleToCamera(Transform transform)
    {
        //Check if this object is in the bounds of the viewport, Z included.
        Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }

    void Grapple()
    {
        if(isReady)
        {
            mPlayer.GetComponent<Rigidbody>().useGravity = false;
            State++;
            isReady = false;
        }
      
    }

    void onEnable()
    {
        controls.PlayerControls.Enable();
    }

    void onDisable()
    {
        controls.PlayerControls.Disable();
    }
}
