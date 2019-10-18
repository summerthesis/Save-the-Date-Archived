using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointScript : MonoBehaviour
{
    public Transform checkpoint;
    void Start()
    {
        checkpoint = gameObject.transform;
    }
}
