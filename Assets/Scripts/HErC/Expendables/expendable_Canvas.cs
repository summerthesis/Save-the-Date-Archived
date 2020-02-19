/******************************************************************************************
 * HErC (Hercules Dias Campos)
 * Save the Date
 * 
 * ********** EXPENDABLE CANVAS CLASS **********
 * 
 * FOR UI INTERACTIVITY ONLY
 * ITS CONTENTS WILL HAVE TO BE MERGED INTO OTHER CLASSES THAT CONTROL INTERFACE!!!
 * 
 * Created November 14, 2019, by HErC
 * Last Modified Feb 18, 2020, by HErC
 * 
 * - Reads all info from the mediator class (ChargeHandler)
 *      and updates its UI elements accordingly.
 * - Currently, it just turns the crosshairs transparent if not aiming.
 * - Will later implement functionality for when there's a target in range
 *      but charges cannot be exchanged
 * 
 ******************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class expendable_Canvas : MonoBehaviour
{
    private Canvas m_canvas;
    
    private ChargeHandler m_mech; //Tracks charges
    [SerializeField] Animator[] m_lights;

    [SerializeField] Color[] aimColors; /* 5 colors:
                                         *   transparent (for inactive)
                                         *   gray        (no chargeable target)
                                         *   green       (chargeable target in sight, can exchange)
                                         *   red         (chargeable target in sight, cannot exchange)
                                         *   purple      (gravity-moveable target in sight)
                                         */

    //Stuff going on for camera and crosshairs
    private RawImage m_crossHairs;

    void Awake() {

        m_canvas = this.gameObject.GetComponent<Canvas>();
        m_crossHairs = this.gameObject.GetComponentInChildren<RawImage>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_mech = GameObject.FindObjectOfType<ChargeHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        //charge text
        if (!m_mech) m_mech = GameObject.FindObjectOfType<ChargeHandler>();

        UpdateArmLights();

        //new code -modified by Hyukin
        CameraBehaviour tempCam = CameraBehaviour.GetInstance();
        if (tempCam.GetIsZooming())
        {
            if (tempCam.GetTarget() == null) m_crossHairs.color = aimColors[1];
            else
            {
                //FUNCTIONALITY THAT ACCOUNTS FOR DIFFERENT TYPES OF TARGETS
                if (tempCam.GetTarget().GetComponent<Chargeable>() != null)
                {
                    m_crossHairs.color = aimColors[2];
                }
                if (tempCam.GetTarget().GetComponent<Moveable>() != null)
                {
                    m_crossHairs.color = aimColors[4];
                }
            }
        }
        else
        {
            m_crossHairs.color = aimColors[0];
        }
    }

    /// <summary>
    /// UI light update function.
    /// Basically stacks lights on based on the number of charges
    /// Uses animations and triggers to fo its job.
    /// OBS: The control bool "LightIsOn" needs to be set after
    ///         setting the trigger
    /// </summary>
    private void UpdateArmLights() {

        switch (m_mech.Charges) {
            case 0:
                for (int i = 0; i < m_mech.MaxCharges; ++i) {
                    if (m_lights[i].GetBool("LightIsOn")) {
                        m_lights[i].SetBool("LightIsOn", false);
                        m_lights[i].SetTrigger("TurnOff");
                    }
                }
                break;
            case 1:
            case 2:
            case 3:
                for (int j = 0; j < m_mech.Charges; ++j) {
                    if (!m_lights[j].GetBool("LightIsOn")) {
                        m_lights[j].SetTrigger("TurnOn");
                        m_lights[j].SetBool("LightIsOn", true);
                    }
                }
                for (int k = m_mech.Charges; k < m_mech.MaxCharges; ++k) {
                    if (m_lights[k].GetBool("LightIsOn")) {
                        m_lights[k].SetTrigger("TurnOff");
                        m_lights[k].SetBool("LightIsOn", false);
                    }
                }
                break;
            default:
                for (int i = 0; i < m_mech.MaxCharges; ++i) {
                        m_lights[i].SetTrigger("TurnOff");
                        m_lights[i].SetBool("LightIsOn", false);
                }
                break;
        }
    }
}
