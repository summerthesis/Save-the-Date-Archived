using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class expendable_Pickup : MonoBehaviour
{
    private Rigidbody m_rb;
    [SerializeField] private float m_rotation;

    void Awake() {
        m_rb = this.gameObject.GetComponent<Rigidbody>();
        m_rotation = 25;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_rb.AddTorque(Vector3.up*m_rotation);
    }

    void OnTriggerEnter(Collider other) {

        PlayerStats temp = other.gameObject.GetComponent<PlayerStats>();
        //if the player is fully charged, the pickup will refuse to charge it
        if (temp) {
            temp.AddGear();
            //alternatively, play sound before destroying
            this.gameObject.SetActive(false);
            Destroy(this.gameObject, 1.0f);
        }
    }
}
