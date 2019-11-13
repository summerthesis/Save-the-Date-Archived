using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mechArmInput : MonoBehaviour
{
    [SerializeField] private float m_thunderDistance;
    public float ThunderDistance { get { return m_thunderDistance; } }
    [SerializeField] private float m_chargeRadius;
    public float ChargeRadius { get { return m_chargeRadius; } }
    [SerializeField] private int m_maxCharges;
    public int MaxCharges { get { return m_maxCharges; } }
    [SerializeField] private int m_charges; //FOR VISUALIZATION ONLY
    public int Charges { get { return m_charges; } }
    [SerializeField] private LineRenderer m_lr;

    void Awake() {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shock() {

    }

    void Discharge() { }

    void Recharge() { }
}
