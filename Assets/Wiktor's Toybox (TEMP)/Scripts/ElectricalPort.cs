using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalPort : MonoBehaviour
{
    public bool isCharged = false;
    public bool isActive = false;

    void Start()
    {
        if (isCharged)
            GetComponent<Renderer>().material.color = Color.red;
        else
            GetComponent<Renderer>().material.color = Color.blue;
    }

    void OnMouseDown()
    {
        if (!isActive)
        {
            isCharged = !isCharged;
            isActive = !isActive;

            if (isCharged)
                GetComponent<Renderer>().material.color = Color.red;
            else
                GetComponent<Renderer>().material.color = Color.blue;
        }
    }
}
