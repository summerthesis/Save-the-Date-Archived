/******************************************************************************************
 * HErC (Hercules Dias Campos)
 * Save the Date
 * 
 * ********** EXPENDABLE UI LIGHTS CLASS **********
 * 
 * FOR UI INTERACTIVITY ONLY
 * 
 * Created Feb 18, 2020, by HErC
 * Last Modified Feb 18, 2020, by HErC
 * 
 *  For Light Behaviours
 ******************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILights : MonoBehaviour
{
    private Animator m_anim;

    void Awake() {
        m_anim = this.gameObject.GetComponent<Animator>();
    }

    public void OnLightOn() {
        m_anim.SetBool("LightIsOn", true);
    }

    public void OnLightOff() {
        m_anim.SetBool("LightIsOn", false);
    }
}
