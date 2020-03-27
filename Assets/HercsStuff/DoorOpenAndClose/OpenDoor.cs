/***************************************************************
 *DOOR OPENER CLASS
 * Created for Ashkan.
 * 
 * When total chargeable on array of ports are charged, 
 * a trigger is set on the door and it opens
 * 
 **************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private Chargeable[] m_ports;
    private int chargedPorts;
    private Animator m_anim;
    private bool hasOpened;

    private void Awake()
    {
        m_anim = this.gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!hasOpened)
        {
            for (int i = 0; i < m_ports.Length; ++i)
            {
                if (m_ports[i].Charged) { ++chargedPorts; }
                else { --chargedPorts; }
                if (chargedPorts < 0) { chargedPorts = 0; }
                else if (chargedPorts > m_ports.Length) { chargedPorts = m_ports.Length; }
            }
            if (chargedPorts == m_ports.Length)
            {
                m_anim.SetTrigger("OpenDoor");
                hasOpened = true;
            }
        }
    }
}