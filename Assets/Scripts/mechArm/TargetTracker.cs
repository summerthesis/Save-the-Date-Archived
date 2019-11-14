using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TargetTracker : MonoBehaviour //May be simplified later
{
    [SerializeField] private float m_chargeRadius;
    private SphereCollider m_sphereCol;

    private bool m_targetInRange;
    public bool TargetInRange { get { return m_targetInRange; } }

    private Chargeable m_chargeable;
    public Chargeable Charger { get { return m_chargeable ; } }

    void Awake()
    {
        m_targetInRange = false;
        m_sphereCol = this.gameObject.GetComponent<SphereCollider>();
        m_sphereCol.radius = m_chargeRadius;
    }
    
    //relatively naive implementation. May require refinement later
    void OnTriggerEnter(Collider other)
    {
        m_chargeable = other.GetComponent<Chargeable>();

        if (m_chargeable)
        {
            m_targetInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {

        m_chargeable = other.GetComponent<Chargeable>();

        if (m_chargeable)
        {
            m_targetInRange = false;
            m_chargeable = null;
        }
    }
}
