/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * Charge Handler
 * Created:       Feb 24, 2019
 * Last Modified: Feb 24, 2019
 * 
 * Inherits from MonoBehaviour
 * 
 * - Tracks destructible objects and shoots them.
 * 
 ********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{

    private PlayerInputAction m_laserAction;
    private bool m_bShoot;
    private Transform targetTransform;

    void Awake() {

        m_laserAction = new PlayerInputAction();

        m_laserAction.PlayerControls.Shoot.performed += ctx => m_bShoot = true;
        m_laserAction.PlayerControls.Shoot.canceled += ctx => m_bShoot = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Implement laser behaviour
        if (m_bShoot) {
            m_bShoot = false;
        }
    }

    private void OnEnable() { m_laserAction.Enable(); }

    private void OnDisable() { m_laserAction.Disable(); }
}
