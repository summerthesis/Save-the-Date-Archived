using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChargeTracker))]
[RequireComponent(typeof(TargetTracker))]
public class ChargeHandler : MonoBehaviour
{
    private TargetTracker m_tTracker;
    private ChargeTracker m_chTracker;

    private ThunderCaster m_thunder;
    private Transform m_targetTransform;
    public Transform TargetTransform { get { return m_targetTransform; } }

    private Light tempLight;

    void Awake() {

        m_targetTransform = null;
        tempLight = this.gameObject.GetComponent<Light>();
        m_tTracker = this.gameObject.GetComponent<TargetTracker>();
        m_chTracker = this.gameObject.GetComponent<ChargeTracker>();
    }

    void Start() {

        m_thunder = this.gameObject.GetComponentInChildren<ThunderCaster>();
    }

    void Update() {
        //TODO: Improve particle system playing
        
        tempLight.enabled = m_tTracker.TargetInRange;

        if (m_tTracker.TargetInRange) {

            m_targetTransform = m_tTracker.Charger.gameObject.transform;
            if (m_thunder) m_thunder.SetTarget(m_targetTransform);

            if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire2")) {

                ExchangeCharges();
            }
        }
        else {

            m_targetTransform = null;
            if (m_thunder) m_thunder.SetTarget(m_targetTransform);
        }
    }

    void ExchangeCharges() {

        if (m_tTracker.Charger.Charged && m_chTracker.CanRecharge) {
            if (m_thunder) m_thunder.CastThunder();
            m_tTracker.Charger.ReturnCharge();
            m_chTracker.Recharge();
        }
        else if (m_tTracker.Charger.Charged && !m_chTracker.CanRecharge) {
            //ping error: both full
        }
        else if (!m_tTracker.Charger.Charged && m_chTracker.CanDischarge) {
            if (m_thunder) m_thunder.CastThunder();
            m_tTracker.Charger.Charge();
            m_chTracker.Discharge();
        }
        else if(!m_tTracker.Charger.Charged && !m_chTracker.CanDischarge){
            //ping error: both empty
        }
    }
}
