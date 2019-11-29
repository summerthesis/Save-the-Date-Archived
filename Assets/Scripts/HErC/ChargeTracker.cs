/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * Charge Tracker
 * Created: November 14, 2019
 * Last Modified: November 19, 2019
 *  
 * Inherits from MonoBehaviour
 * 
 * - Tracks the Mechanical Arm's number of charges
 * 
 * - Takes care of the Mechanical Arm's recharge/discharge processes
 * 
 ********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTracker : MonoBehaviour
{
    [SerializeField] private int numberOfCharges; //number of charges
    public int Charges { get { return numberOfCharges; } }

    [SerializeField] private int maxCharges; //max charges
    public int MaxCharges { get { return maxCharges; } }

    //booleans: Mechanical arm's ability to be recharge/charge others
    public bool CanRecharge { get { return numberOfCharges < maxCharges; } }
    public bool CanDischarge { get { return numberOfCharges > 0; } }

    /// <summary>
    /// Recharge function.
    /// - Checks whether the mechanical arm can be recharged
    ///     - if not, returns false without altering charges
    ///     - if yes, updates number of charges and returns true
    /// </summary>
    /// <param name="charges"></param>
    /// <returns></returns>
    public bool Recharge(int charges = 1)
    {
        if (numberOfCharges == maxCharges)
        {
            return false;
        }
        
        //extra charges will be spent
        numberOfCharges += charges;
        if (numberOfCharges >= maxCharges)
        {
            numberOfCharges = maxCharges;
        }

        return true;
    }

    /// <summary>
    /// Discharge function.
    /// - Checks whether the mechanical arm has charges to use
    ///     - if not, returns false without changing charge number
    ///     - otherwise, updates charge number and returns true
    /// </summary>
    /// <returns></returns>
    public bool Discharge()
    {
        if (numberOfCharges == 0)
        {
            return false;
        }

        --numberOfCharges;
        return true;
    }
}
