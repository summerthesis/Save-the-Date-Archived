/*********************************************************************
 * HErC (Hercules Dias Campos)
 * Save The Date
 *
 * Moveable
 * Created: November 29, 2019
 * Last Modified: November 29, 2019
 *  
 * Inherits from MonoBehaviour
 * 
 * - Moveable script for objects controllable with the gravity gun
 * 
 * - MOVES USING RIGIDBODY'S POSITION!!!
 * 
 * - As of now, UI will have a purple component to indicate whether it's active or not
 * 
 ********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Moveable : MonoBehaviour
{
    public Rigidbody MyRigidbody; //made public so player can access
    //Control variables
    private bool isAfloat;
    public bool Afloat { get { return isAfloat; } }

    [SerializeField] private Transform innerTarget; //temporarily serialized for visualization purposes

    //temporary variable for floatiness
    private bool addDist;

    void Awake()
    {
        addDist = false;
        MyRigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAfloat && !addDist)
        {
            MyRigidbody.position += Vector3.up * 0.5f;
        }
        addDist = isAfloat;
    }

    /// <summary>
    /// function that sets the rigidbody afloat
    /// for now, a naive implementation
    /// </summary>
    /// <param name="afloat"></param>
    public void SetAfloat(Transform target) {

        innerTarget = target;
        if (target)
        {
            MyRigidbody.position = target.position;
            MyRigidbody.rotation = target.rotation;
            isAfloat = true;
        }
        else
        {
            isAfloat = false;
        }

        MyRigidbody.useGravity = (innerTarget == null);
    }
}
