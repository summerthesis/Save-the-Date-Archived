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

    private Interactable m_interactable;
    public Interactable Interactor { get { return m_interactable; } }

    void Awake()
    {
        m_targetInRange = false;
        m_sphereCol = this.gameObject.GetComponent<SphereCollider>();
        m_sphereCol.radius = m_chargeRadius;
    }
    
    //relatively naive implementation. May require refinement later
    void OnTriggerEnter(Collider other)
    {

        m_interactable = other.GetComponent<Interactable>();

        if (m_interactable)
        {
            m_targetInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {

        m_interactable = other.GetComponent<Interactable>();

        if (m_interactable)
        {
            m_targetInRange = false;
            m_interactable = null;
        }
    }
}
