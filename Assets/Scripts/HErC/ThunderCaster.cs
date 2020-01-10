/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * Thunder Caster
 * Created: November 14, 2019
 * Last Modified: November 18, 2019
 *  
 * Inherits from MonoBehaviour
 * 
 * - Handles functionality pertaining the particle system
 * 
 * - Has a public function to handle setting of its target object's transform
 *      This was done to decouple functionality, and have classes be mediated
 *      exclusively by the Charge Handler class, without knowing the existence
 *      of other classes that may be present in the Mechanical Arm's GameObject
 *      
 * - The basic functionality of this class is to orient the emitter 
 *      and fire it when appropriate
 * 
 ********************************************************************/
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderCaster : MonoBehaviour
{
    private ParticleSystem particleSystem; //particle system
    [SerializeField] private Transform targetTransform; //for visualization purposes

    /// <summary>
    /// Constructor functionality. Gets the Particle System component from the GameObject,
    /// and nullifies the target's transform
    /// </summary>
    void Awake()
    {
        particleSystem = this.gameObject.GetComponent<ParticleSystem>();
        targetTransform = null;
    }

    /// <summary>
    /// Basically, looks at the target if there is one
    /// </summary>
    void Update()
    {
        if (targetTransform)
        {
            this.gameObject.transform.LookAt(targetTransform);
        }
    }

    /// <summary>
    /// Accessible from the outside, this function
    /// was designed to assign a target to this GameObject
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }

    /// <summary>
    /// Fires the particle system if it's not already playing
    /// </summary>
    public void CastThunder()
    {
        if (!particleSystem.isPlaying) particleSystem.Play();
    }

    /// <summary>
    /// Function designed to stop the particle system, 
    /// should it be necessary to do so
    /// </summary>
    public void StopCasting()
    {
        //Stop (if needed)
    }
}
