/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * Laser Beahaviour
 * Created:       Feb 24, 2019
 * Last Modified: Feb 24, 2019
 * 
 * Inherits from MonoBehaviour
 * 
 * - Tracks destructible objects when zoom is applied
 * - Orients particle emitter towards target
 * - DOES NOT YET FIRE EMITTERS
 * 
 ********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{

    private PlayerInputAction m_laserAction;
    private bool m_bShoot;
    
    [SerializeField] private Transform m_ParticleTransform;

    private Transform m_targetTransform;

    void Awake() {

        m_laserAction = new PlayerInputAction();

        m_laserAction.PlayerControls.ElecArm.performed += ctx => m_bShoot = true;
        m_laserAction.PlayerControls.ElecArm.canceled += ctx => m_bShoot = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Implement laser behaviour
        m_targetTransform = CameraBehaviour.GetInstance().GetTarget();
        if (m_targetTransform && m_targetTransform.GetComponent<Destructible>() != null) {
            m_ParticleTransform.LookAt(m_targetTransform); //This is the basis for particle
            if (m_bShoot) {
                m_bShoot = false;
                Destroy(m_targetTransform.gameObject);
                m_targetTransform = null;
            }
        }
    }

    private void OnEnable() { m_laserAction.Enable(); }

    private void OnDisable() { m_laserAction.Disable(); }
}
