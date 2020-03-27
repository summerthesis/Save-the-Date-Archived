using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    PlayerInputAction controls;

    public GameObject PlayerObject;
    public GameObject HandLocation;
    public GameObject PlayerCamera;
    public GameObject CameraBody;
    public Material IdleMaterial;
    public Material AvailableMaterial;
    public Material ActiveMaterial;
    public GameObject CurrentTarget;
    public List<GameObject> MagCollectables;
    private bool isHit, hasTarget;
    private string hitObjectTag;
    private bool pulling;
    void Awake()
    {
        controls = new PlayerInputAction();
        controls.PlayerControls.ActivateMagArm.started += ctx => pulling = true;
        controls.PlayerControls.ActivateMagArm.canceled += ctx => pulling = false;
        controls.PlayerControls.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        MagCollectables = new System.Collections.Generic.List<GameObject>(); 
        MagCollectables.AddRange(GameObject.FindGameObjectsWithTag("MagCollectable"));
    }

    // Update is called once per frame
    void Update()
    {
        IsLookingAt("MagCollectable");
        if (CurrentTarget)
        {
            CurrentTarget.GetComponent<MeshRenderer>().material = AvailableMaterial;
        }
        else
        {
            if(hasTarget)
            {
                for (int i = 0; i < MagCollectables.Count; i++)
                {
                    MagCollectables[i].GetComponent<MeshRenderer>().material = IdleMaterial;
                    hasTarget = false;
                }

            }
           
        }
        if(pulling) { MagPull(CurrentTarget); }

    } 
    
    void MagArm()
    {
     
    }

    void MagPull(GameObject target)
    {
        if (!CurrentTarget) return;
        if(Vector3.Distance(CurrentTarget.transform.position, HandLocation.transform.position) < 1.65f)
        {
            if(CurrentTarget.tag == "MagCollectable")
            {
                for(int i = 0; i < MagCollectables.Count; i++)
                {
                    if(CurrentTarget == MagCollectables[i])
                    {
                        Destroy(CurrentTarget); CurrentTarget = null;
                        MagCollectables.RemoveAt(i);
                        return;
                    }
                  
                }
            }
            
        }
        else
        {
            CurrentTarget.GetComponent<MeshRenderer>().material = ActiveMaterial;
            CurrentTarget.transform.position = Vector3.MoveTowards(CurrentTarget.transform.position, HandLocation.transform.position, 15.0f * Time.deltaTime);
        }

    }
    public bool IsLookingAt(string tag, float tolerance = 0.9f)//Must give interactable objects a tag that you can pass here.
    {
        if (tag == "MagCollectable")
        {
            for(int i = 0; i < MagCollectables.Count; i++)
            {
                Vector3 direction = (MagCollectables[i].transform.position - PlayerCamera.transform.position).normalized;
                float dot = Vector3.Dot(direction, PlayerCamera.transform.forward);
                if(dot > tolerance)
                {
                    CurrentTarget = MagCollectables[i];
                    hasTarget = true;
                    return true;
                }
                else { CurrentTarget = null; }
            }
        }

        return false;
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
