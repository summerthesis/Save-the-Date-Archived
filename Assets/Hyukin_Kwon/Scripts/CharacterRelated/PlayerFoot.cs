using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    private bool m_bIsGrounded;

    public bool GetIsGrounded() { return m_bIsGrounded; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            m_bIsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            m_bIsGrounded = false;
        }
    }
}
