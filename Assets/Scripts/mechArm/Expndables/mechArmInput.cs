using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mechArmInput : MonoBehaviour
{
    [SerializeField] private float m_thunderDistance;
    public float ThunderDistance { get { return m_thunderDistance; } }

    [SerializeField] private float m_chargeRadius;
    public float ChargeRadius { get { return m_chargeRadius; } }

    private bool m_isNearChargeable;

    //Thunder variables
    [SerializeField] private AudioClip m_thunderSFX;
    [SerializeField] private AudioClip m_rechargeSFX;
    //these two can be the same
    [SerializeField] private AudioClip m_nochargeSFX;
    [SerializeField] private AudioClip m_noRechargeSFX;

    [SerializeField] private LineRenderer m_lr;
    private SphereCollider m_collider; //THIS IS INTERACTING WITH PICKUPS

    private ParticleSystem m_psTemp; //THIS IS A TEST

    void Awake() {

        m_psTemp = this.gameObject.GetComponent<ParticleSystem>();
        m_collider = this.gameObject.GetComponent<SphereCollider>();
        m_collider.radius = m_chargeRadius;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TESTS
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Fire2")) GenerateSparks();
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire3")) Electrify();
    }

    void GenerateSparks() {
        
        m_psTemp.Play();
    }

    void Electrify() {

        //this is where the distance check and decision will go.
        //It'll either call Shock(), Discharge(), or Recharge()
    }
    void Shock() {
        //shocks target
    }

    //may need to be reworked into boolean
    
    void OnTriggerEnter(Collider other) {

    }
}
