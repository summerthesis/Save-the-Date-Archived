using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTracker : MonoBehaviour
{
    [SerializeField] private int m_charges;
    public int Charges { get { return m_charges; } }

    [SerializeField] private int m_maxCharges;
    public int MaxCharges { get { return m_maxCharges; } }

    public bool CanRecharge { get { return m_charges < m_maxCharges; } }
    public bool CanDischarge { get { return m_charges > 0; } }

    public bool Recharge(int charges = 1) {
        if (m_charges == m_maxCharges) return false;
        
        //extra charges will be spent
        m_charges += charges;
        if (m_charges >= m_maxCharges) m_charges = m_maxCharges;

        return true;
    }

    public bool Discharge() {

        if (m_charges == 0) return false;

        --m_charges;
        return true;
    }
}
