using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChargeTracker))]
[RequireComponent(typeof(TargetTracker))]
public class ChargeHandler : MonoBehaviour
{
    private TargetTracker m_tTracker;
    private ChargeTracker m_chTracker;

    private Light tempLight;

    void Awake() {
        tempLight = this.gameObject.GetComponent<Light>();
        m_tTracker = this.gameObject.GetComponent<TargetTracker>();
        m_chTracker = this.gameObject.GetComponent<ChargeTracker>();
    }

    void Update() {
        tempLight.enabled = m_tTracker.TargetInRange;

        if (m_tTracker.TargetInRange) {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire2")) {
                ExchangeCharges();
            }
        }
        
    }

    void ExchangeCharges() {

        if (m_tTracker.Interactor.Charged && m_chTracker.CanRecharge) {
            m_tTracker.Interactor.ReturnCharge();
            m_chTracker.Recharge();
        }
        else if (m_tTracker.Interactor.Charged && !m_chTracker.CanRecharge) {
            //ping error: both full
        }
        else if (!m_tTracker.Interactor.Charged && m_chTracker.CanDischarge) {
            m_tTracker.Interactor.Charge();
            m_chTracker.Discharge();
        }
        else if(!m_tTracker.Interactor.Charged && !m_chTracker.CanDischarge){
            //ping error: both empty
        }
    }
}
