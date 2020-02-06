/******************************************************************************************
 * HErC (Hercules Dias Campos)
 * Save the Date
 * 
 * ********** EXPENDABLE CANVAS CLASS **********
 * 
 * FOR UI INTERACTIVITY ONLY
 * ITS CONTENTS WILL HAVE TO BE MERGED INTO OTHER CLASSES THAT CONTROL INTERFACE!!!
 * 
 * Created November 14, 2019
 * Last Modified Jan 17, 2020 by Hyukin Kwon
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
    private Text m_text;
    private ChargeHandler m_mech;
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
        m_text = this.gameObject.GetComponentInChildren<Text>();
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

        //old code
        //if (m_mech.Aiming)
        //{
        //    if (m_mech.TargetTransform == null) m_crossHairs.color = aimColors[1];
        //    else {
        //        //FUNCTIONALITY THAT ACCOUNTS FOR DIFFERENT TYPES OF TARGETS
        //        if (m_mech.TargetTransform.GetComponent<Chargeable>() != null)
        //        {
        //            m_crossHairs.color = aimColors[2];
        //        }
        //        if (m_mech.TargetTransform.GetComponent<Moveable>() != null)
        //        {
        //            m_crossHairs.color = aimColors[4];
        //        }
        //    }
        //}
        //new code -modified by Hyukin

        CameraMovement tempCam = CameraMovement.GetInstance();
        if (tempCam.GetIsAiming())
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

        m_text.text = m_mech.Charges + "/" + m_mech.MaxCharges;
    }
}
