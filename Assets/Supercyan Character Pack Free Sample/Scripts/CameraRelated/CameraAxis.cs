using System.Collections;
using UnityEngine;

public class CameraAxis : MonoBehaviour
{
    [SerializeField] Transform m_playerAxis;
    [SerializeField] Transform m_aimingAxis;
    [SerializeField] float m_followingSpeed;
    bool m_isAiming = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            m_isAiming =! m_isAiming;

        if(!m_isAiming)
        {
            transform.position = Vector3.Lerp(transform.position, m_playerAxis.position, m_followingSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, m_aimingAxis.position, m_followingSpeed * Time.deltaTime);
        }
    }
}