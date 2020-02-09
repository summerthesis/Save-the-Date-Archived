/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * Charge Handler
 * Created: November 14, 2019
 * Last Modified: February 09, 2019
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
 * - FEB 09 Mod.: Removed Material functionality from the code. The related
 *                functionality will be moved to an expendable class
 * 
 ********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chargeable : MonoBehaviour
{    
    //allows designers to choose whether the object will (or not) have a charge at the beginning
    [SerializeField] private bool isCharged; 
    public bool Charged { get { return isCharged; } }

    /// <summary>
    /// Updates "charged" status and assigns material's emission (turns on)
    /// </summary>
    public void Charge()
    {
        isCharged = true;
    }

    /// <summary>
    /// Assigns material's emission (turns off) and updates "charged" status
    /// </summary>
    public void ReturnCharge()
    {
        isCharged = false;
    }
}
