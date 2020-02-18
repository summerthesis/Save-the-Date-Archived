using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalPort : MonoBehaviour
{
    private Chargeable chargeable;
    public bool isCharged { get { return chargeable.Charged; } }
    private bool hasCharged;
    private bool startedRunning;
    public bool Active { get { return hasCharged ^ startedRunning; } }

    void Start()
    {
        startedRunning = false;
        chargeable = this.gameObject.GetComponent<Chargeable>();
        GetComponent<Renderer>().material.color = chargeable.Charged ? Color.red : Color.blue;
        hasCharged = chargeable.Charged;
    }

    void Update()
    {
        hasCharged = chargeable.Charged;
        if (chargeable) GetComponent<Renderer>().material.color = chargeable.Charged ? Color.red : Color.blue;
        else GetComponent<Renderer>().material.color = Color.blue;
    }

    public void SetActiveState(bool activeState)
    {
        startedRunning = activeState;
    }
}
