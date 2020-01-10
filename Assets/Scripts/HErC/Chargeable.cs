/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * Charge Handler
 * Created: November 14, 2019
 * Last Modified: November 18, 2019
 *  
 * Inherits from MonoBehaviour
 * 
 * - Tracks the component's internal charge, and makes this information
 *   available
 * 
 * - It also (as of now) creates an emission colour for the chargeable
 *   object (primitive UI indicator). 
 *   May be replaced or done with as needed
 * 
 ********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chargeable : MonoBehaviour
{
    private Material chargeableMaterial; //material.
    [SerializeField] private Color emissionColour; //selectable. May be removed if needed
    
    //allows designers to choose whether the object will (or not) have a charge at the beginning
    [SerializeField] private bool isCharged; 
    public bool Charged { get { return isCharged; } }

    /// <summary>
    /// Constructor functionality: assigns the material, for later tracking
    /// </summary>
    void Awake()
    {
        chargeableMaterial = this.gameObject.GetComponent<MeshRenderer>().material;
    }

    /// <summary>
    /// Updates the material's emission in the shader, based on the presence of a charge
    /// This aspect of the material may be replaced with a particle system to indicate that
    /// the object is charged, but that's for designers to decide. 
    /// Once they do, the corresponding code must be changed accordingly
    /// </summary>
    void Update()
    {
        //update appearance based on charge
        //chargeableMaterial.color = isCharged ? Color.green : Color.gray;
        //_EmissionColor is the glow. May be best used in combination with halo
        chargeableMaterial.SetColor("_EmissionColor", isCharged ? emissionColour : Color.black);
    }

    /// <summary>
    /// Updates "charged" status and assigns material's emission (turns on)
    /// </summary>
    public void Charge()
    {
        isCharged = true;
        chargeableMaterial.SetColor("_EmissionColor", emissionColour);
    }

    /// <summary>
    /// Assigns material's emission (turns off) and updates "charged" status
    /// </summary>
    public void ReturnCharge()
    {
        chargeableMaterial.SetColor("_EmissionColor", Color.black);
        isCharged = false;
    }
}
