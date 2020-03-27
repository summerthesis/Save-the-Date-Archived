/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * EXPENDABLE Material Handler
 * Created: February 09, 2019
 * Last Modified: February 09, 2019
 *  
 * Inherits from MonoBehaviour
 * 
 * - Changes material colour (emissive) based on the charged state of the object.
 * 
 * - FOR VISUAL FEEDBACK PURPOSES ONLY. 
 *      Real in-game objects will have their own behaviour when charged
 * 
 ********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Chargeable))]
public class expendableMatHandler : MonoBehaviour
{
    [SerializeField] private Color m_emissive;
    private Material m_mat;
    private Chargeable m_charge;

    void Awake() {
        m_charge = this.gameObject.GetComponent<Chargeable>();
        m_mat = this.gameObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        m_mat.SetColor("_EmissionColor", m_charge.Charged ? m_emissive : Color.black);
    }
}
