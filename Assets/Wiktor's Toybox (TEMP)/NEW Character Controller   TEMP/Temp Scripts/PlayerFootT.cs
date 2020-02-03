using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootT : MonoBehaviour
{
    private bool m_bIsGrounded;

    public bool GetIsGrounded() { return m_bIsGrounded; }
    public void SetIsGrounded(bool b) { m_bIsGrounded = b; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player" && !m_bIsGrounded && other.transform.tag != "HookPoint")
        {
            m_bIsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag != "Player" && other.transform.tag != "HookPoint")
        {
            m_bIsGrounded = false;
        }
    }
}
